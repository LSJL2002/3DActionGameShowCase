using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    private float targetModifier;
    private float accelerationTime;

    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // SO에서 가속 시간 + 최대 Modifier 가져오기
        accelerationTime = stateMachine.GroundData.RunAccelerationTime;   // 걷기→달리기까지 시간
        targetModifier = 0f;

        // 초기값 세팅
        stateMachine.MovementSpeedModifier = 0f;
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

        // 목표 Modifier = 입력 세기 (0~1)
        targetModifier = inputMagnitude;

        // 점진적으로 목표 Modifier로 이동
        float modifierStep = 1f / accelerationTime * Time.deltaTime;
        stateMachine.MovementSpeedModifier = Mathf.MoveTowards(
            stateMachine.MovementSpeedModifier,
            targetModifier,
            modifierStep
        );

        // 입력 없으면 Idle 전환
        if (inputMagnitude == 0f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}