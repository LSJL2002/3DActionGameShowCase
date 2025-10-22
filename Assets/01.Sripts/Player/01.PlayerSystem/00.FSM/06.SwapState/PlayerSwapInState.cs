using UnityEngine;

public class PlayerSwapInState : PlayerBaseState
{
    public PlayerSwapInState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.SwapInParameterHash);
        sm.Player.Animator.CrossFade("Swap_In", 0.1f, 0);

        sm.Player.EnableCharacterInput(true);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.SwapInParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(sm.Player.Animator, "SwapIn");
        if (normalizedTime >= 1f)
        {
            sm.Player.Ability.FinishSwap();
        }

        if (sm.MovementInput.sqrMagnitude > 0.01f)
            sm.Player.Ability.FinishSwap();
    }
}