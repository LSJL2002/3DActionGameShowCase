using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerDodgeState : Istate
{
    private PlayerStateMachine stateMachine;
    private float dodgeDuration = 0.8f; // 회피 지속 시간
    private float startTime;
    private float dodgeDistance = 3f;    // 뒤로 밀리는 거리
    private int dodgeLayerIndex;
    private Vector3 dodgeDirection;
    private int dodgeDir; // Animator int 파라미터


    public PlayerDodgeState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        // Layer 이름으로 인덱스 가져오기
        dodgeLayerIndex = stateMachine.Player.Animator.GetLayerIndex("Dodge Layer");
    }

    public void Enter()
    {
        // 시작 시간 기록
        startTime = Time.time;
        Vector2 input = stateMachine.MovementInput;

        if (input.sqrMagnitude < 0.01f)
        {
            // 입력 없으면 뒤로
            dodgeDirection = -stateMachine.Player.transform.forward;
            dodgeDir = 0; // 뒤 회피 애니메이션
        }
        else
        {
            // 입력 있으면 플레이어 기준 이동 방향
            dodgeDirection = stateMachine.Player.transform.forward * input.y +
                             stateMachine.Player.transform.right * input.x;
            dodgeDirection.y = 0;
            dodgeDirection.Normalize();

            dodgeDir = 1; // 입력 있음 → int1 애니메이션
        }

        var anim = stateMachine.Player.Animator;
        anim.SetLayerWeight(dodgeLayerIndex, 1f); // Dodge Layer 켜기
        anim.SetInteger(stateMachine.Player.AnimationData.DodgeDirParameterHash, dodgeDir);
        anim.SetTrigger(stateMachine.Player.AnimationData.DodgeParameterHash);

        // 무적 적용
        stateMachine.IsInvincible = true;
    }

    public void Exit()
    {
        var anim = stateMachine.Player.Animator;
        anim.SetLayerWeight(dodgeLayerIndex, Mathf.Lerp(
            anim.GetLayerWeight(dodgeLayerIndex), 0f, Time.deltaTime * 0f)); // Layer 끄기
        anim.SetInteger(stateMachine.Player.AnimationData.DodgeDirParameterHash, 0);

        stateMachine.IsInvincible = false;
    }

    public void HandleInput() { }

    public void LogicUpdate()
    {
        // RootMotion은 Animator에서 이미 적용됨
        // → 여기서는 보정치만 더해줌

        float extraDistance = 0f;

        if (dodgeDir == 0) // 뒤로 회피
        {
            // 뒤로 모션은 RootMotion 거의 없음 → 강하게 밀어줌
            extraDistance = 3f;
        }
        else if (dodgeDir == 1) // 앞으로 회피
        {
            // 앞으로 모션은 RootMotion 이미 있음 → 살짝만 밀어줌
            extraDistance = 1f;
        }
        else if (dodgeDir == 2 || dodgeDir == 3) // 좌/우
        {
            // 옆으로는 RootMotion 살짝만 → 적당히 보정
            extraDistance = 2f;
        }

        // ForceMove로 Dodge 이동
        float moveSpeed = dodgeDistance / dodgeDuration;
        stateMachine.Player.Controller.Move(dodgeDirection * moveSpeed * Time.deltaTime);

        // Dodge 종료 체크
        if (Time.time >= startTime + dodgeDuration)
        {
            Exit();

            // 입력 있으면 Walk, 없으면 Idle
            if (stateMachine.MovementInput.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(stateMachine.WalkState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void PhysicsUpdate() { }
}
