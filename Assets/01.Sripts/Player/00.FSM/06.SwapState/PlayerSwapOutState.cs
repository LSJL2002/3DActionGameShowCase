using UnityEngine;

public class PlayerSwapOutState : PlayerBaseState
{

    public PlayerSwapOutState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        sm.Player.Animator.SetTrigger(sm.Player.AnimationData.SwapOutParameterHash);
        sm.Player.EnableCharacterInput(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(sm.Player.Animator, "SwapOut");
        if (normalizedTime >= 1f)
        {
            StateComplete(); // 이벤트 발생
        }
    }
}
