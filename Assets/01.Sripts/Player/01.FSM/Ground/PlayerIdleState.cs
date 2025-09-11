using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);



        // IdleState 진입 시 currentSpeedModifier 리셋
        stateMachine.MovementSpeedModifier = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(stateMachine.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
}