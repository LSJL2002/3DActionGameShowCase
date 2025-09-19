using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerDodgeState : Istate
{
    //Istate로 정의돼 있으면 LogicUpdate, PhysicsUpdate, HandleInput 같은 공통 메서드를 모든 상태에서 동일하게 호출
    //상태머신에서 상태 전환할 때 타입에 상관없이 일관되게 동작하게 하려면 인터페이스가 필요합니다.

    private PlayerStateMachine stateMachine;
    private float dodgeDuration = 0.8f; // 애니메이션 길이만큼 유지
    private float startTime;
    private int dodgeLayerIndex;

    public PlayerDodgeState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        // Layer 이름으로 인덱스 가져오기
        dodgeLayerIndex = stateMachine.Player.Animator.GetLayerIndex("Overall/Toggle_DodgeLayer");
    }

    public void Enter()
    {
        // 시작 시간 기록
        startTime = Time.time;

        var anim = stateMachine.Player.Animator;
        anim.SetLayerWeight(dodgeLayerIndex, 1f);
        anim.SetTrigger(stateMachine.Player.AnimationData.DodgeParameterHash);

        // 무적
        stateMachine.IsInvincible = true;

        // 루트모션 + 추가 힘
        Vector2 input = stateMachine.MovementInput;
        Vector3 dodgeDir;

        if (stateMachine.MovementInput.sqrMagnitude < 0.01f)
            dodgeDir = -stateMachine.Player.transform.forward; // 뒤로 회피
        else
            dodgeDir = stateMachine.Player.transform.forward;  // 바라보는 방향

        // 원하는 거리만큼 힘 추가
        float dodgeStrength = 5f;
        stateMachine.Player.ForceReceiver.AddForce(dodgeDir * dodgeStrength, horizontalOnly: true);
    }

    public void Exit()
    {
        var anim = stateMachine.Player.Animator;
        anim.SetLayerWeight(dodgeLayerIndex, 0f);

        stateMachine.IsInvincible = false;
    }

    public void HandleInput() { }

    public void LogicUpdate()
    {
        // 단순히 시간만 보고 종료
        if (Time.time >= startTime + dodgeDuration)
        {
            Exit();

            if (stateMachine.MovementInput.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(stateMachine.WalkState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void PhysicsUpdate()
    {
        // ForceReceiver에서 계산된 힘 적용
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime); // 실제 이동 적용
    }
}
