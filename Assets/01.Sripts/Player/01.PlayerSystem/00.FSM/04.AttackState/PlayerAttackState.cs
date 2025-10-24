using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private BattleModule module;

    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.AttackBoolHash);
        sm.Player.Ability.StartAttack();

        module = sm.CurrentBattleModule;
        module.OnAttackEnd += HandleAttackEnd;

        sm.Player.Motor.AllowMovement = false;
        sm.Player.Motor.AllowRotation = false;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.AttackBoolHash);
        sm.Player.Ability.EndAttack();

        module.OnAttackEnd -= HandleAttackEnd;
        module.OnAttackCanceled();
        module.ResetCombo();
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