using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.DieParameterHash);

        sm.MovementInput = Vector2.zero;
        sm.Player.EnableCharacterInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.DieParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float normalizedTime = GetNormalizeTime(sm.Player.Animator, "Die");
        if (normalizedTime >= 1f)
        {
            sm.Player.Ability.EndDeath();
            sm.Player.gameObject.SetActive(false);
        }
    }
}