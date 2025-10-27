using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.GroundBoolHash);
        sm.Player.Motor.AllowMovement = true;
        sm.Player.Motor.AllowInput = true;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.GroundBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}