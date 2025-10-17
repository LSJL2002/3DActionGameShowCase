using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        sm.JumpForce = sm.Player.InfoData.AirData.JumpForce;
        sm.Player.ForceReceiver.Jump(sm.JumpForce);
        base.Enter();
        StartAnimation(sm.Player.AnimationData.JumpTriggerHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.JumpTriggerHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (sm.Player.Controller.velocity.y <= 0)
        {
            sm.ChangeState(sm.FallState);
            return;
        }
    }
}