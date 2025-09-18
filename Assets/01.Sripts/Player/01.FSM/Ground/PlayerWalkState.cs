using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    private float transitionTimer = 0f; // 걷기 유지 시간 카운터
    private float blend = 0f;           // 0 -> 1 Blend

    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        transitionTimer = 0f;
        blend = 0f;

        // 초기 Blend값 (Animator용)
        stateMachine.MovementSpeedModifier = 0f;

        // Animator 속도 초기화
        stateMachine.Player.Animator.speed = stateMachine.GroundData.BaseSpeed * stateMachine.GroundData.WalkSpeedModifier;
    }

    public override void Exit()
    {
        base.Exit();

        // Animator 속도 기본값으로 리셋
        stateMachine.Player.Animator.speed = 1f;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        float inputMagnitude = stateMachine.MovementInput.magnitude;

        // 입력 없으면 Idle 상태로 전환
        if (inputMagnitude <= 0.01f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // Blend 값 증가 (0 -> 1)
        transitionTimer += Time.deltaTime;
        float accelerationTime = stateMachine.GroundData.RunAccelerationTime;
        blend = Mathf.Clamp01(transitionTimer / accelerationTime);

        // Animator용 Blend 적용 (0~1)
        stateMachine.MovementSpeedModifier = blend;

        // 실제 이동 속도 계산: BaseSpeed * Lerp(WalkSpeedModifier, RunSpeedModifier, Blend)
        float animatorSpeed = stateMachine.GroundData.BaseSpeed *
                          Mathf.Lerp(stateMachine.GroundData.WalkSpeedModifier,
                                     stateMachine.GroundData.RunSpeedModifier,
                                     blend);

        stateMachine.Player.Animator.speed = animatorSpeed;

        // 루트모션 이동 (deltaPosition 그대로 사용)
        Vector3 deltaPosition = stateMachine.Player.Animator.deltaPosition;
        deltaPosition.y = 0f; // 수직 이동 제거
        stateMachine.Player.Controller.Move(deltaPosition);
    }
}