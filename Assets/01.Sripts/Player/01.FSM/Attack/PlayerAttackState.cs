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
    private bool forceApplied = false;

    private Transform attackTarget;

    // Dash 관련
    private float dashSpeed = 10f;
    private float stopDistance = 1.5f;

    public override PlayerStateID StateID => PlayerStateID.Attack;
    public override bool AllowRotation => false;

    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();

        // PlayerInfo SO에서 공격 데이터 가져오기
        currentAttack = stateMachine.Player.InfoData.AttackData.GetAttackInfoData(stateMachine.ComboIndex);

        // 근처 타겟 탐지
        attackTarget = FindNearestMonster(currentAttack.AttackRange);
        stateMachine.Player.Combat.SetAttackTarget(attackTarget);

        // 공격 시작 시 Trigger + Bool 세팅
        var anim = stateMachine.Player.Animator;
        anim.SetTrigger(stateMachine.Player.AnimationData.AttackTriggerHash);
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        StartAnimation(stateMachine.Player.AnimationData.ComboBoolHash);

        SetAttack(stateMachine.ComboIndex); // ComboIndex만 적용

        bufferedComboIndex = -1;
        stateMachine.IsAttacking = true;
        forceApplied = false;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        StopAnimation(stateMachine.Player.AnimationData.ComboBoolHash);

        attackButtonHeld = false;
        bufferedComboIndex = -1;
        forceApplied = false;
        stateMachine.IsAttacking = false;
        attackTarget = null;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        HandleDashAndForce(normalizedTime);
        HandleComboBuffer(normalizedTime);
        HandleAttackEnd(normalizedTime);
    }


    private void HandleDashAndForce(float normalizedTime)
    {
        CharacterController cc = stateMachine.Player.GetComponent<CharacterController>();

        float dashSpeedValue = currentAttack.DashSpeed;
        float stopDistanceValue = currentAttack.StopDistance;

        if (attackTarget != null)
        {
            Vector3 dir = (attackTarget.position - stateMachine.Player.transform.position).normalized;
            float distance = Vector3.Distance(stateMachine.Player.transform.position, attackTarget.position);

            // 타겟 가까이 오면 멈춤
            if (distance > stopDistanceValue)
            {
                cc.Move(dir * dashSpeedValue * Time.deltaTime);
                stateMachine.Player.transform.forward = dir;
            }
        }

        if (!forceApplied && normalizedTime >= currentAttack.ForceTransitionTime)
        {
            forceApplied = true;
            stateMachine.Player.ForceReceiver.Reset();

            // 타겟 방향 (수평만 적용)
            Vector3 dir = attackTarget != null
                ? (attackTarget.position - stateMachine.Player.transform.position)
                : stateMachine.Player.transform.forward;

            dir.y = 0; // 여기서 y 제거
            dir.Normalize();

            // 공격 돌진은 수평으로만
            stateMachine.Player.ForceReceiver.AddForce(dir * currentAttack.Force, true);

            // 바라보는 방향 회전
            stateMachine.Player.transform.forward = dir;

            // 스킬/이펙트 호출
            stateMachine.Player.Combat.OnAttack(currentAttack.AttackName);
        }
    }


    private void HandleComboBuffer(float normalizedTime)
    {
        if (attackButtonHeld && bufferedComboIndex < 0)
        {
            int nextIndex = currentAttack.ComboStateIndex;
            if (nextIndex >= 0 && nextIndex < stateMachine.Player.InfoData.AttackData.GetAttackInfoCount())
            {
                bufferedComboIndex = nextIndex;
            }
        }

        if (bufferedComboIndex >= 0 && normalizedTime >= currentAttack.ComboTransitionTime)
        {
            SetAttack(bufferedComboIndex);
            bufferedComboIndex = -1;
        }
    }

    private void HandleAttackEnd(float normalizedTime)
    {
        if (normalizedTime >= 1f)
        {
            if (bufferedComboIndex >= 0)
            {
                SetAttack(bufferedComboIndex);
                bufferedComboIndex = -1;
                return;
            }

            stateMachine.ComboIndex = 0;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private void SetAttack(int comboIndex)
    {
        stateMachine.ComboIndex = comboIndex;
        stateMachine.SetAttackInfo(comboIndex);
        currentAttack = stateMachine.AttackInfo;

        forceApplied = false;

        // 애니메이션 재시작 없이 ComboInt만 갱신
        stateMachine.Player.Animator.SetInteger(stateMachine.Player.AnimationData.ComboIntHash, comboIndex);
    }


    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        attackButtonHeld = true;
        if (!stateMachine.IsAttacking)
            stateMachine.IsAttacking = true;
    }

    protected override void OnAttackCanceled(InputAction.CallbackContext context)
    {
        attackButtonHeld = false;
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        // 공격 애니메이션 일정 시간 이후 캔슬 가능
        if (normalizedTime >= 0.1f)
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }

    // -------------------
    // 몬스터 탐지 함수
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