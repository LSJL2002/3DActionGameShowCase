using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private BattleModule module;

    private Transform attackTarget;


    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override bool AllowMovement => false; // 스킬 중 이동 제한
    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);

        stateMachine.IsAttacking = true;

        module = stateMachine.CurrentBattleModule;

        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Attack.SetAttackTarget(attackTarget);
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackBoolHash);

        stateMachine.IsAttacking = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        module?.OnUpdate();

        // 콤보가 비활성화되면 Idle로 전환
        if (module?.ComboHandler?.IsActive == false)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        base.OnAttackStarted(context);

        module?.OnAttack();

        // 공격 시작 시점에만 타겟 갱신
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Attack.SetAttackTarget(attackTarget);
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);
    }

    protected override void OnAttackCanceled(InputAction.CallbackContext context)
    {
        base.OnAttackCanceled(context);

        if (stateMachine.CurrentBattleModule is BattleModule_Yuki yuki)
            yuki.OnAttackCanceled();
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        base.OnDodgeStarted(context);
        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");
        if (normalizedTime >= 0.1f)
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }
}