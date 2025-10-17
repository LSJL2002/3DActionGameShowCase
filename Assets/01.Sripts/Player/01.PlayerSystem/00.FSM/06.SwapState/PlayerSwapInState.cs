using UnityEngine;

public class PlayerSwapInState : PlayerBaseState
{

    public PlayerSwapInState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        sm.Player.Animator.CrossFade("Swap_In", 0.1f, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 이동 입력 들어오면 즉시 Idle
        if (sm.MovementInput.sqrMagnitude > 0.01f)
        {
            sm.ChangeState(sm.IdleState);
            return;
        }

        float normalizedTime = GetNormalizeTime(sm.Player.Animator, "SwapIn");
        if (normalizedTime >= 1f)
        {
            StateComplete(); // 이벤트 발생
            sm.ChangeState(sm.IdleState);
        }
    }
}