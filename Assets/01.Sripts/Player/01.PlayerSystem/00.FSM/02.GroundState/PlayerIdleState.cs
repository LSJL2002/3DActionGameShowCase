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

    public PlayerIdleState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.IdleBoolHash);

        sm.MovementSpeedModifier = 0f;

        idleStartTime = Time.time; // Idle 시작 시간 기록
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.IdleBoolHash);

        OnCompleteGettingUp();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 10초 이상 입력 없으면 눕기 모션 트리거
        if (!isWaitingAnimationTriggered && Time.time - idleStartTime >= waitingAnimationDelay)
        {
            sm.Player.Animator.SetTrigger(sm.Player.AnimationData.WaitingAnimationTriggerHash);
            isWaitingAnimationTriggered = true;
            return;
        }
    }

    private void OnCompleteGettingUp()
    {
        isGettingUp = false;
        isWaitingAnimationTriggered = false;
        idleStartTime = Time.time;
    }
}