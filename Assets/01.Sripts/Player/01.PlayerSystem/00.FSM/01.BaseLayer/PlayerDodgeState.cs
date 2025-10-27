using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerDodgeState : PlayerBaseState
{
    //Istate로 정의돼 있으면 LogicUpdate, PhysicsUpdate, HandleInput 같은 공통 메서드를 모든 상태에서 동일하게 호출
    //상태머신에서 상태 전환할 때 타입에 상관없이 일관되게 동작하게 하려면 인터페이스가 필요합니다.

    private readonly float dodgeDuration;     // 상태 유지 시간(초)
    private readonly float dodgeStrength;     // 밀려나는 힘 크기
    private float startTime;
    private int dodgeLayerIndex;

    public PlayerDodgeState(PlayerStateMachine sm) : base(sm)
    {
        // Layer 이름으로 인덱스 가져오기
        dodgeLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_DodgeLayer");

        // GroundData에서 값 가져오기
        dodgeDuration = sm.Player.InfoData.GroundData.DodgeDuration;
        dodgeStrength = sm.Player.InfoData.GroundData.DodgeStrength;
    }

    public override void Enter()
    {
        base.Enter();
        sm.Player.Animator.SetTrigger(sm.Player.AnimationData.DodgeParameterHash);
        sm.Player.Animator.SetLayerWeight(dodgeLayerIndex, 1f);
        sm.Player.Ability.StartDodge();
        sm.Player.Motor.AllowMovement = false;
        sm.Player.Motor.AllowRotation = false;

        startTime = Time.time; // 시작 시간 기록

        // 이동 방향 + 힘
        Vector3 dodgeDir = GetDodgeDirection();
        sm.Player.ForceReceiver.AddForce(dodgeDir * dodgeStrength);

    }

    public override void Exit()
    {
        base.Exit();
        sm.Player.Animator.SetLayerWeight(dodgeLayerIndex, 0f);
        sm.Player.Ability.EndDodge();
    }

    public override void LogicUpdate()
    {
        // Dodge 지속 시간 체크
        if (Time.time >= startTime + dodgeDuration)
        {
            sm.Player.Ability.EndDodge();

            // 현재 SpeedModifier를 저장
            sm.LastWalkBlend = sm.MovementSpeedModifier;
            sm.LastWalkTimer = sm.LastWalkBlend * sm.GroundData.RunAccelerationTime;
        }
    }

    public override void PhysicsUpdate() { }


    // ---- 헬퍼 메서드 ----
    private Vector3 GetDodgeDirection()
    {
        if (sm.MovementInput.sqrMagnitude < 0.01f)
            return -sm.Player.transform.forward; // 뒤로 회피
        else
            return sm.Player.transform.forward;  // 바라보는 방향
    }
}