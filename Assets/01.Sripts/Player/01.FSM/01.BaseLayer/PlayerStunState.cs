using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class PlayerStunState : Istate
{
    private readonly PlayerStateMachine stateMachine;

    private  float duration;
    private readonly int stunLayerIndex;

    private float elapsed;

    public PlayerStunState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

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
        anim.SetLayerWeight(stunLayerIndex, 1);

        stateMachine.IsStun = true;
    }

    public void Exit()
    {
        var anim = stateMachine.Player.Animator;
        anim.SetLayerWeight(stunLayerIndex, 0);

        stateMachine.IsStun = false;
    }

    public void HandleInput()
    {
        // 스턴 중 입력 무시
    }


    public void LogicUpdate()
    {
        Debug.Log(duration);

        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Exit();
            // 스턴 종료 → Idle로 복귀
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void PhysicsUpdate()
    {
        // 스턴 중 이동 없음 (ForceReceiver 사용 안 함)
    }
}
