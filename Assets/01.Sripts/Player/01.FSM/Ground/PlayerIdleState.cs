using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    private float idleStartTime;
    private float waitingAnimationDelay = 15f; // 10초 후 대기 모션
    private bool isWaitingAnimationTriggered = false;
    private bool isGettingUp = false;

    // Idle 상태에서는 기본적으로 회전 금지
    public override bool AllowRotation => !isWaitingAnimationTriggered && !isGettingUp;
    public override bool AllowMovement => !isWaitingAnimationTriggered && !isGettingUp;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleBoolHash);

        stateMachine.MovementSpeedModifier = 0f;
        stateMachine.ComboIndex = 0;

        idleStartTime = Time.time; // Idle 시작 시간 기록

        // Move 입력 이벤트 등록
        stateMachine.Player.Input.PlayerActions.Move.started += OnMoveStarted;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleBoolHash);

        stateMachine.Player.Input.PlayerActions.Move.started -= OnMoveStarted;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // LogicUpdate에서는 트리거 재호출 방지
        if (isGettingUp)
        {
            float normTime = GetNormalizeTime(stateMachine.Player.Animator, "GetUp");
            if (normTime >= 0.99f)
            {
                isGettingUp = false;
                isWaitingAnimationTriggered = false;
                idleStartTime = Time.time;
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            return;
        }

        Vector2 moveInput = stateMachine.MovementInput;

        // 10초 이상 입력 없으면 눕기 모션 트리거
        if (!isWaitingAnimationTriggered && Time.time - idleStartTime >= waitingAnimationDelay)
        {
            stateMachine.Player.Animator.SetTrigger(stateMachine.Player.AnimationData.WaitingAnimationTriggerHash);
            isWaitingAnimationTriggered = true;
            return;
        }

        // Idle 상태에서 입력 발생 → Walk 전환
        if (!isWaitingAnimationTriggered && moveInput.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        if (!isWaitingAnimationTriggered)
        {
            // 일반 Idle → Walk
            stateMachine.ChangeState(stateMachine.WalkState);
        }
        else if (isWaitingAnimationTriggered && !isGettingUp)
        {
            // 눕기 모션 중 입력 → 일어나기 모션 실행
            stateMachine.Player.Animator.SetTrigger(stateMachine.Player.AnimationData.GetUpTriggerHash);
            isGettingUp = true;
        }
    }
}