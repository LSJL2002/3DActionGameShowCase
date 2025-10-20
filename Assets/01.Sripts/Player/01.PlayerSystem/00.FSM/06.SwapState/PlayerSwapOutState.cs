using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSwapOutState : PlayerBaseState
{
    public PlayerSwapOutState(PlayerStateMachine sm) : base(sm) { }

    public void SetNextCharacter(PlayerCharacter newChar) { }

    public override void Enter()
    {
        base.Enter();
        sm.Player.Animator.CrossFade("Swap_Out", 0.1f, 0);
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
            // FSM에 스왑 완료 알림
            sm.Player.Ability.FinishSwap();
            sm.Player.gameObject.SetActive(false);
        }
    }
}