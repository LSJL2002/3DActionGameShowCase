using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    protected AttackInfoData attackData; // 현재 공격 정보 캐싱

    private bool alreadyAppliedCombo;
    private bool alreadyAppliedDamage;
    private bool forceApplied;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // ComboIndex 기준 AttackInfo 세팅
        if (stateMachine.AttackInfo == null)
            stateMachine.SetAttackInfo(stateMachine.ComboIndex);

        SetAttack(stateMachine.ComboIndex);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        // Force 적용 + 공격처리
        if (!forceApplied && normalizedTime >= attackData.ForceTransitionTime)
        {
            forceApplied = true;
            stateMachine.Player.ForceReceiver.Reset();
            stateMachine.Player.ForceReceiver.AddForce(
                stateMachine.Player.transform.forward * attackData.Force
            );
        }

        // 데미지 판정
        if (!alreadyAppliedDamage && normalizedTime >= attackData.Dealing_Start_TransitionTime)
        {
            alreadyAppliedDamage = true;
            stateMachine.Player.Combat.PerformAttack(attackData.AttackName);
        }


        // 콤보 처리
        if (!alreadyAppliedCombo && normalizedTime >= attackData.ComboTransitionTime)
        {
            if (stateMachine.IsAttacking && attackData.ComboStateIndex != -1)
            {
                alreadyAppliedCombo = true;
                stateMachine.ComboIndex = attackData.ComboStateIndex;
                SetAttack(stateMachine.ComboIndex);
                return;
            }
        }


        // 애니메이션 끝났는데 콤보 입력 없으면 Idle
        if (normalizedTime >= 1f && !alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private void SetAttack(int comboIndex)
    {
        stateMachine.SetAttackInfo(comboIndex);
        attackData = stateMachine.AttackInfo;

        alreadyAppliedCombo = false;
        alreadyAppliedDamage = false;
        forceApplied = false;

        // 공격 애니메이션 파라미터 세팅
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex);

        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);
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