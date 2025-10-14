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

        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Attack.SetAttackTarget(attackTarget);
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);

        module = stateMachine.CurrentBattleModule;
        if (module != null) module.OnAttackEnd += HandleAttackEnd;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackBoolHash);

        stateMachine.IsAttacking = false;

        if (module != null) module.OnAttackEnd -= HandleAttackEnd;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        module?.OnUpdate();
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

    private void HandleAttackEnd()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}