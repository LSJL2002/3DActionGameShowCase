using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine sm) : base(sm) { }


    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.AirBoolHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.AirBoolHash);
    }
}
