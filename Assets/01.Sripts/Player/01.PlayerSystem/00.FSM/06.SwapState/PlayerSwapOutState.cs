using UnityEngine;

public class PlayerSwapOutState : PlayerBaseState
{
    public PlayerSwapOutState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        // 입력 초기화
        sm.MovementInput = Vector2.zero;
        sm.Player.EnableCharacterInput(false);

        StartAnimation(sm.Player.AnimationData.SwapOutParameterHash);
        sm.Player.Animator.CrossFade("Swap_Out", 0.1f, 0);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.SwapOutParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float normalizedTime = GetNormalizeTime(sm.Player.Animator, "SwapOut");
        if (normalizedTime >= 1f)
        {
            // FSM에 스왑 완료 알림
            sm.Player.Ability.FinishSwap();
            sm.Player.gameObject.SetActive(false);
        }
    }
}