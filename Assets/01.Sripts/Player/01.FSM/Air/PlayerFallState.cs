using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    private LayerMask groundLayer;
    private float fallDelay = 0.15f; // 잠깐 지연
    private float airStartTime;

    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        // 바닥 레이어 설정 (필요에 맞게 수정)
        groundLayer = LayerMask.GetMask("Default");
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallBoolHash);

        airStartTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 잠깐 지연
        if (Time.time - airStartTime < fallDelay)
            return;

        // 바닥 체크
        if (IsGrounded())
        {
            if (stateMachine.MovementInput.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(stateMachine.WalkState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private bool IsGrounded()
    {
        CharacterController cc = stateMachine.Player.Controller;

        // 기본 isGrounded 체크
        if (cc.isGrounded)
            return true;

        // Raycast 체크
        Ray ray = new Ray(stateMachine.Player.transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f, groundLayer))
        {
            // 바닥 각도 확인
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle <= cc.slopeLimit)
                return true; // 계단이나 슬로프면 grounded로 간주
        }

        return false; // 진짜 떨어질 때만 Fall
    }
}
