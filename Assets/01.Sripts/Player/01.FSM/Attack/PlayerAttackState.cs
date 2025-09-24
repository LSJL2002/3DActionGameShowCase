using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private AttackInfoData currentAttack;
    private int bufferedComboIndex = -1;

    private bool attackButtonHeld = false;
    private bool attackEnded = false;
    private bool comboTriggered = false; // 트리거 한 번만 발동

    private Transform attackTarget;
    private float lastAttackInputTime;
    private readonly float idleTimeout = 1f;

    public override PlayerStateID StateID => PlayerStateID.Attack;
    public override bool AllowRotation => false; // 공격 중 캐릭터 회전은 ApplyAttackMovement에서 처리

    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        InitializeAttack();
    }

    public override void Exit()
    {
        base.Exit();
        ResetAttackState();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        ApplyAttackMovement(normalizedTime);  // 이동/돌진 처리
        HandleCombo(normalizedTime);          // 콤보 처리
        HandleAttackEnd(normalizedTime);      // 공격 종료 체크
        CheckIdleTransition();                // Idle 상태 전환 체크
    }

    // ===================== 공격 초기화 =====================
    private void InitializeAttack()
    {
        currentAttack = stateMachine.Player.InfoData.AttackData.GetAttackInfoData(stateMachine.ComboIndex);

        // 가장 가까운 몬스터 탐색
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange);
        stateMachine.Player.Combat.SetAttackTarget(attackTarget);
        // 공격 진입 시 Lock-On 강제 적용
        if (attackTarget != null)  stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);

        var anim = stateMachine.Player.Animator;
        //anim.SetTrigger(stateMachine.Player.AnimationData.AttackTriggerHash);
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        // 첫 공격 트리거
        stateMachine.Player.Animator.SetTrigger(stateMachine.Player.AnimationData.AttackTriggerHash);

        SetAttack(stateMachine.ComboIndex);

        bufferedComboIndex = -1;
        attackEnded = false;
        comboTriggered = false;
        attackButtonHeld = true;
        stateMachine.IsAttacking = true;
        lastAttackInputTime = Time.time;
    }

    private void ResetAttackState()
    {
        StopAnimation(stateMachine.Player.AnimationData.AttackBoolHash);

        bufferedComboIndex = -1;
        attackButtonHeld = false;
        attackEnded = false;
        comboTriggered = false;
        attackTarget = null;
        stateMachine.IsAttacking = false;
    }

    // ===================== 공격 이동 & Force =====================
    private void ApplyAttackMovement(float normalizedTime)
    {
        if (attackTarget == null) return;

        //몬스터 바라보기만 적용
        Vector3 dir = (attackTarget.position - stateMachine.Player.transform.position).normalized;
        dir.y = 0;
        dir.Normalize();

        stateMachine.Player.transform.forward = dir;
        // Force 제거: 제자리 공격
        // ApplyForce() 호출하지 않음
    }

    // ApplyForce()도 사용하지 않으므로 빈 메서드로 두거나 삭제 가능
    private void ApplyForce()
    {
        // 이제 Force 효과 제거
    }

    // ===================== 공격 콤보 처리 =====================
    private void HandleCombo(float normalizedTime)
    {
        // 다음 공격 버퍼링
        if (attackButtonHeld && bufferedComboIndex < 0 && currentAttack.ComboStateIndex != -1)
            bufferedComboIndex = currentAttack.ComboStateIndex;

        // ComboTransitionTime 이상 + 트리거 미발동
        if (bufferedComboIndex >= 0 && normalizedTime >= currentAttack.ComboTransitionTime && !comboTriggered)
        {
            comboTriggered = true;
            stateMachine.Player.Animator.SetTrigger(stateMachine.Player.AnimationData.ComboTriggerHash);
            SetAttack(bufferedComboIndex);
            bufferedComboIndex = -1;
        }

        // 마지막 공격(-1) 처리
        if (currentAttack.ComboStateIndex == -1 && normalizedTime >= 1f)
        {
            if (attackButtonHeld)
            {
                SetAttack(1); // 입력 유지 시 1타부터 재시작
            }
            // 입력 없으면 FinishAttackState로 CheckIdleTransition에서 전환
        }
    }
    private void HandleAttackEnd(float normalizedTime)
    {
        if (!attackEnded && normalizedTime >= 1f)
        {
            attackEnded = true;
        }
    }
    private void CheckIdleTransition()
    {
        if (attackButtonHeld)
            lastAttackInputTime = Time.time;

        if (attackEnded)
        {
            // 이동 입력이 있으면 WalkState
            if (stateMachine.MovementInput.sqrMagnitude > 0.01f)
            {
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            // 이동 입력이 없고 idleTimeout 경과 시 FinishAttack
            else if (Time.time - lastAttackInputTime >= idleTimeout)
            {
                stateMachine.ChangeState(stateMachine.FinishAttackState);
            }
        }
    }
    private void SetAttack(int comboIndex)
    {
        stateMachine.ComboIndex = comboIndex;
        stateMachine.SetAttackInfo(comboIndex);
        currentAttack = stateMachine.AttackInfo;

        attackEnded = false;
        comboTriggered = false; // 트리거 재발동 가능

        // Animator 파라미터 업데이트
        stateMachine.Player.Animator.SetInteger(stateMachine.Player.AnimationData.ComboIntHash, comboIndex);
    }

    // ===================== 입력 처리 =====================
    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        attackButtonHeld = true;
        lastAttackInputTime = Time.time;
        if (!stateMachine.IsAttacking)
            stateMachine.IsAttacking = true;

        // 트리거는 여기서만 발동
        stateMachine.Player.Animator.SetTrigger(stateMachine.Player.AnimationData.ComboTriggerHash);
    }

    protected override void OnAttackCanceled(InputAction.CallbackContext context)
    {
        attackButtonHeld = false;
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");
        if (normalizedTime >= 0.1f)
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }


    // ===================== 타겟 탐지 =====================
    private Transform FindNearestMonster(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(stateMachine.Player.transform.position, radius, LayerMask.GetMask("Enemy"));
        if (hits.Length == 0) return null;

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(stateMachine.Player.transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }
        return nearest;
    }
}