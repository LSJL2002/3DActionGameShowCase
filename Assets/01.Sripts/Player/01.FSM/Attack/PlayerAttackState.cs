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

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // ComboIndex 기준 AttackInfo 세팅
        if (stateMachine.AttackInfo == null)
            stateMachine.SetAttackInfo(stateMachine.ComboIndex);

        // 공격 시작 시 Trigger + Bool 세팅
        var anim = stateMachine.Player.Animator;
        anim.SetTrigger(stateMachine.Player.AnimationData.AttackTriggerHash);
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        StartAnimation(stateMachine.Player.AnimationData.ComboBoolHash);

        SetAttack(stateMachine.ComboIndex); // ComboIndex만 적용

        bufferedComboIndex = -1;
        stateMachine.IsAttacking = true;
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        HandleForce(normalizedTime);
        HandleComboBuffer(normalizedTime);
        HandleAttackEnd(normalizedTime);
    }

    private void HandleForce(float normalizedTime)
    {
        if (!forceApplied && normalizedTime >= currentAttack.ForceTransitionTime)
        {
            forceApplied = true;
            stateMachine.Player.ForceReceiver.Reset();
            stateMachine.Player.ForceReceiver.AddForce(
                stateMachine.Player.transform.forward * currentAttack.Force
            );
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
}