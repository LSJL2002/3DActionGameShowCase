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
    private bool attackEnded = false;

    private Transform attackTarget;

    private float lastAttackInputTime;
    private float attackEndTime;
    private readonly float idleTimeout = 1f;


    public override PlayerStateID StateID => PlayerStateID.Attack;
    public override bool AllowRotation => false;

    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();

        // 현재 공격 데이터 세팅
        currentAttack = stateMachine.Player.InfoData.AttackData.GetAttackInfoData(stateMachine.ComboIndex);

        // 타겟 탐지 & 세팅
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange);
        stateMachine.Player.Combat.SetAttackTarget(attackTarget);

        // 애니메이션 시작
        var anim = stateMachine.Player.Animator;
        anim.SetTrigger(stateMachine.Player.AnimationData.AttackTriggerHash);
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        StartAnimation(stateMachine.Player.AnimationData.ComboBoolHash);

        SetAttack(stateMachine.ComboIndex);

        bufferedComboIndex = -1;
        forceApplied = false;
        attackEnded = false;

        stateMachine.IsAttacking = true;
        lastAttackInputTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        StopAnimation(stateMachine.Player.AnimationData.ComboBoolHash);

        attackButtonHeld = false;
        bufferedComboIndex = -1;
        forceApplied = false;
        attackEnded = false;
        attackTarget = null;

        stateMachine.IsAttacking = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        ApplyAttackMovement(normalizedTime);
        HandleCombo(normalizedTime);
        HandleAttackEnd(normalizedTime);
        CheckIdleTransition();
    }


    private void ApplyAttackMovement(float normalizedTime)
    {
        var cc = stateMachine.Player.Controller;
        float dashSpeed = stateMachine.Player.InfoData.AttackData.DashSpeed;
        float stopDistance = stateMachine.Player.InfoData.AttackData.StopDistance;

        // Dash 처리
        if (attackTarget != null)
        {
            Vector3 dir = (attackTarget.position - stateMachine.Player.transform.position).normalized;
            float distance = Vector3.Distance(stateMachine.Player.transform.position, attackTarget.position);

            if (distance > stopDistance)
            {
                cc.Move(dir * dashSpeed * Time.deltaTime);
                stateMachine.Player.transform.forward = dir;
            }
        }

        // Force 적용
        if (!forceApplied && normalizedTime >= currentAttack.ForceTransitionTime)
        {
            ApplyForce();
        }
    }

    private void ApplyForce()
    {
        forceApplied = true;
        stateMachine.Player.ForceReceiver.Reset();

        Vector3 dir = attackTarget != null
            ? (attackTarget.position - stateMachine.Player.transform.position)
            : stateMachine.Player.transform.forward;

        dir.y = 0;
        dir.Normalize();

        stateMachine.Player.ForceReceiver.AddForce(dir * currentAttack.Force, true);
        stateMachine.Player.transform.forward = dir;

        stateMachine.Player.Combat.OnAttack(currentAttack.AttackName);
    }


    private void HandleCombo(float normalizedTime)
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
        if (!attackEnded && normalizedTime >= 1f)
        {
            attackEnded = true;
            attackEndTime = Time.time;
        }

        if (attackEnded && bufferedComboIndex >= 0)
        {
            SetAttack(bufferedComboIndex);
            bufferedComboIndex = -1;
            attackEnded = false;
        }
    }


    private void CheckIdleTransition()
    {
        if (attackButtonHeld)
            lastAttackInputTime = Time.time;

        if (Time.time - lastAttackInputTime >= idleTimeout)
        {
            stateMachine.ComboIndex = 0;
            stateMachine.ChangeState(stateMachine.FinishAttackState);
        }
    }


    private void SetAttack(int comboIndex)
    {
        stateMachine.ComboIndex = comboIndex;
        stateMachine.SetAttackInfo(comboIndex);
        currentAttack = stateMachine.AttackInfo;

        forceApplied = false;
        stateMachine.Player.Animator.SetInteger(stateMachine.Player.AnimationData.ComboIntHash, comboIndex);
    }


    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        attackButtonHeld = true;
        lastAttackInputTime = Time.time;
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
        if (normalizedTime >= 0.1f)
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }


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