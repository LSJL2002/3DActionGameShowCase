using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.DieParameterHash);
        sm.Player.Animator.CrossFade("Die", 0.1f, 0);

        sm.MovementInput = Vector2.zero;
        sm.Player.EnableCharacterInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(sm.Player.AnimationData.DieParameterHash);
        // 나중에 부활은 고민좀 해봐야될듯
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