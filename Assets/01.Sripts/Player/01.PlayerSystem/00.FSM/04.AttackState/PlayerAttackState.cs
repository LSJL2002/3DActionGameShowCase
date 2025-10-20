using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private BattleModule module;

    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override bool AllowMovement => false; // 스킬 중 이동 제한
    public override bool AllowRotation => false;

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.AttackBoolHash);
        sm.Player.Ability.StartAttack();

        module = sm.CurrentBattleModule;
        module.OnAttackEnd += HandleAttackEnd;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.AttackBoolHash);

        module.OnAttackEnd -= HandleAttackEnd;
        module.OnAttackCanceled();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        module?.OnUpdate();
    }

    private void HandleAttackEnd()
    {
        sm.Player.Ability.EndAttack();
    }
}