using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class PlayerStunState : Istate
{
    private readonly PlayerStateMachine stateMachine;

    private  float duration;
    private readonly float layerBlendSpeed;
    private readonly int stunLayerIndex;

    private float startTime;
    private float currentLayerWeight = 0f;
    private bool targetLayerOn = false;
    private float elapsed;

    public PlayerStunState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.layerBlendSpeed = layerBlendSpeed;

        // Layer 이름 → Animator에서 찾기
        stunLayerIndex = stateMachine.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
    }

    /// <summary>
    /// 외부(몬스터)에서 스턴 시간 설정
    /// </summary>
    public void Setup(float time)
    {
        duration = time;
        elapsed = 0f;
    }

    public void Enter()
    {
        // 애니메이션 트리거
        var anim = stateMachine.Player.Animator;
        anim.SetTrigger(stateMachine.Player.AnimationData.StunParameterHash);

        startTime = Time.time;
        targetLayerOn = true;

        stateMachine.IsStun = true;
    }

    public void Exit()
    {
        targetLayerOn = false;
        stateMachine.IsStun = false;
    }

    public void HandleInput()
    {
        // 스턴 중 입력 무시
    }


    public void LogicUpdate()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Exit();
            // 스턴 종료 → Idle로 복귀
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        UpdateLayerWeight();
    }

    public void PhysicsUpdate()
    {
        // 스턴 중 이동 없음 (ForceReceiver 사용 안 함)
    }

    private void UpdateLayerWeight()
    {
        float target = targetLayerOn ? 1f : 0f;
        currentLayerWeight = Mathf.MoveTowards(
            currentLayerWeight,
            target,
            layerBlendSpeed * Time.deltaTime
        );

        stateMachine.Player.Animator.SetLayerWeight(stunLayerIndex, currentLayerWeight);
    }
}
