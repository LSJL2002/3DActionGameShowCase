using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerDodgeState : Istate
{
    //Istate로 정의돼 있으면 LogicUpdate, PhysicsUpdate, HandleInput 같은 공통 메서드를 모든 상태에서 동일하게 호출
    //상태머신에서 상태 전환할 때 타입에 상관없이 일관되게 동작하게 하려면 인터페이스가 필요합니다.

    //Base를 상속받으면 입력 처리나 공통 이동 로직을 그대로 쓸수있음
    //하지만 입력 이동 로직이 아예 필요 없는 특수 상태.
    private PlayerStateMachine sm;

    // ---- 설정 값 ----
    private readonly float DodgeDuration;   // 상태 유지 시간(초)
    private readonly float DodgeStrength;     // 밀려나는 힘 크기
    private readonly float LayerBlendSpeed;   // 레이어 페이드 속도

    // ---- 내부 상태 ----
    private float startTime;
    private int dodgeLayerIndex;
    private float currentLayerWeight = 0f;
    private bool targetLayerOn = false;


    public PlayerDodgeState(PlayerStateMachine sm)
    {
        this.sm = sm;
        // Layer 이름으로 인덱스 가져오기
        dodgeLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_DodgeLayer");

        // GroundData에서 값 가져오기
        DodgeDuration = sm.Player.InfoData.GroundData.DodgeDuration;
        DodgeStrength = sm.Player.InfoData.GroundData.DodgeStrength;
        LayerBlendSpeed = sm.Player.InfoData.GroundData.DodgeLayerBlendSpeed;
    }

    public void Enter()
    {
        var anim = sm.Player.Animator;
        anim.SetTrigger(sm.Player.AnimationData.DodgeParameterHash);
        sm.Player.Ability.StartDodge();

        startTime = Time.time; // 시작 시간 기록
        targetLayerOn = true;

        // 이동 방향 + 힘
        Vector3 dodgeDir = GetDodgeDirection();
        sm.Player.ForceReceiver.AddForce(dodgeDir * DodgeStrength, horizontalOnly: true);
    }

    public void Exit()
    {
        sm.Player.Ability.EndDodge();

        targetLayerOn = false;
    }

    public void HandleInput() { }

    public void LogicUpdate()
    {
        // Dodge 지속 시간 체크
        if (Time.time >= startTime + DodgeDuration)
        {
            sm.Player.Ability.EndDodge();

            if (sm.MovementInput.sqrMagnitude > 0.01f) // MovementInput이 있으면 WalkState로
            {
                // 현재 SpeedModifier를 저장
                sm.LastWalkBlend = sm.MovementSpeedModifier;
                sm.LastWalkTimer = sm.LastWalkBlend * sm.GroundData.RunAccelerationTime;
            }
        }

        UpdateLayerWeight();
    }

    public void PhysicsUpdate()
    {
        // ForceReceiver에서 계산된 힘 적용
        sm.Player.Controller.Move(sm.Player.ForceReceiver.Movement * Time.deltaTime); // 실제 이동 적용
    }


    // ---- 헬퍼 메서드 ----
    private Vector3 GetDodgeDirection()
    {
        if (sm.MovementInput.sqrMagnitude < 0.01f)
            return -sm.Player.transform.forward; // 뒤로 회피
        else
            return sm.Player.transform.forward;  // 바라보는 방향
    }

    private void UpdateLayerWeight()
    {
        float target = targetLayerOn ? 1f : 0f;

        // MoveTowards: 목표값까지 일정 속도로 이동
        currentLayerWeight = Mathf.MoveTowards(
            currentLayerWeight,
            target,
            LayerBlendSpeed * Time.deltaTime
        );

        sm.Player.Animator.SetLayerWeight(dodgeLayerIndex, currentLayerWeight);
    }
}