using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    // 블렌드값 관련
    protected float maxSpeedModifier = 1f;   // 최대 블렌드값
    protected float accelerationTime = 2f;    // 2초 동안 최대 속도 도달


    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 입력 세기
        float inputMagnitude = stateMachine.MovementInput.magnitude;
        // 목표 속도
        float targetSpeed = inputMagnitude * maxSpeedModifier;

        // 입력이 있으면 0부터 서서히 증가
        float speedStep = (maxSpeedModifier / accelerationTime) * Time.deltaTime;
        stateMachine.MovementSpeedModifier = Mathf.MoveTowards(
            stateMachine.MovementSpeedModifier, targetSpeed, speedStep);
    }
}