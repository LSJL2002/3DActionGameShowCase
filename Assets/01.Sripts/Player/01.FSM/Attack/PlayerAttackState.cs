using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    protected AttackInfoData attackData; // 필드로 선언

    private bool alreadyAppliedCombo;
    private bool alreadyAppliedDamage;
    private bool forceApplied;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SetAttack(stateMachine.ComboIndex);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);

        // 공격 종료 후 이동 속도 원복
        stateMachine.MovementSpeedModifier = 1f;

        // Hitbox 확실히 비활성화
        stateMachine.Player.Combat.EndAttack();
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
        if (!alreadyAppliedDamage && normalizedTime >= attackData.HitTime)
        {
            alreadyAppliedDamage = true;
            stateMachine.Player.Combat.PerformAttack(stateMachine.Player.Stats.Attack);
        }

        // 콤보 입력 가능 구간
        if (!alreadyAppliedCombo && normalizedTime >= attackData.ComboTransitionTime)
        {
            if (stateMachine.IsAttacking && attackData.ComboStateIndex != -1)
            {
                alreadyAppliedCombo = true;
                stateMachine.ComboIndex = attackData.ComboStateIndex;

                // 같은 AttackState 안에서 다음 콤보 공격 실행
                SetAttack(stateMachine.ComboIndex);
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
        attackData = stateMachine.CurrentAttackInfo;

        alreadyAppliedCombo = false;
        alreadyAppliedDamage = false;
        forceApplied = false;

        // 공격 애니메이션 파라미터 세팅
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex);
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);

        // 이동 속도 제한 (루트모션이 있으면 필요없음)
        stateMachine.MovementSpeedModifier = 0f;
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");

        // 공격 애니메이션 일정 시간 이후 캔슬 가능
        if (normalizedTime >= 0.3f)
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }
}