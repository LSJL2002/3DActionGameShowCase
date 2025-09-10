using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // Shift 누르면 달리기 속도 적용
        if (stateMachine.Player.Input.PlayerActions.Run.IsPressed())
            stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        else
            stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
    }
}