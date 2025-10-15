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
        StartAnimation(sm.Player.AnimationData.IdleBoolHash);

        sm.MovementSpeedModifier = 0f;
        sm.ComboIndex = 0;

        idleStartTime = Time.time; // Idle 시작 시간 기록

        // Move 입력 이벤트 등록
        sm.Player.Input.PlayerActions.Move.started += OnMoveStarted;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.IdleBoolHash);

        sm.Player.Input.PlayerActions.Move.started -= OnMoveStarted;
        OnCompleteGettingUp();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // LogicUpdate에서는 트리거 재호출 방지
        if (isGettingUp)
        {
            float normTime = GetNormalizeTime(sm.Player.Animator, "GetUp");
            if (normTime >= 0.99f)
            {
                OnCompleteGettingUp();
                sm.ChangeState(sm.WalkState);
            }
            return;
        }

        Vector2 moveInput = sm.MovementInput;

        // 10초 이상 입력 없으면 눕기 모션 트리거
        if (!isWaitingAnimationTriggered && Time.time - idleStartTime >= waitingAnimationDelay)
        {
            sm.Player.Animator.SetTrigger(sm.Player.AnimationData.WaitingAnimationTriggerHash);
            isWaitingAnimationTriggered = true;
            return;
        }

        // Idle 상태에서 입력 발생 → Walk 전환
        if (!isWaitingAnimationTriggered && moveInput.sqrMagnitude > 0.01f)
        {
            sm.ChangeState(sm.WalkState);
        }
    }

    private void OnCompleteGettingUp()
    {
        isGettingUp = false;
        isWaitingAnimationTriggered = false;
        idleStartTime = Time.time;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        if (!isWaitingAnimationTriggered)
        {
            // 일반 Idle → Walk
            sm.ChangeState(sm.WalkState);
        }
        else if (isWaitingAnimationTriggered && !isGettingUp)
        {
            // 눕기 모션 중 입력 → 일어나기 모션 실행
            sm.Player.Animator.SetTrigger(sm.Player.AnimationData.GetUpTriggerHash);
            isGettingUp = true;
        }
    }
}