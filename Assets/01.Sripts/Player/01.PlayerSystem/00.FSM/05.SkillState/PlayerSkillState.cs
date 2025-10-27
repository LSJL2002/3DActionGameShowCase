using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private BattleModule module;

    public PlayerSkillState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.SkillBoolHash);
        sm.Player.Ability.StartSkill();

        module = sm.CurrentBattleModule;
        module.OnSkillEnd += HandleSkillEnd;

        sm.Player.Motor.AllowMovement = false;
        sm.Player.Motor.AllowRotation = false;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.SkillBoolHash);
        sm.Player.Ability.EndSkill();

        module.OnSkillEnd -= HandleSkillEnd;
        module.OnSkillCanceled();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        module?.OnSkillUpdate();
    }

    private void HandleSkillEnd()
    {
        sm.Player.Ability.EndSkill();
    }
}