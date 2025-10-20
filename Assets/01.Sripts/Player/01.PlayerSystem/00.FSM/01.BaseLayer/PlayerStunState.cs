using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : Istate
{
    private readonly PlayerStateMachine stateMachine;

    private readonly int stunLayerIndex;

    private float duration;
    private float elapsed;

    public PlayerStunState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        stunLayerIndex = stateMachine.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
    }

    /// <summary>
    /// 외부(몬스터)에서 스턴 시간 설정
    /// </summary>
    public void Setup(float time)
    {
        duration = time;

        // 이미 넉백 중이라면 → 다시 시작하도록 시간 초기화
        elapsed = 0f;
    }

    public void Enter()
    {
        var anim = stateMachine.Player.Animator;
        anim.SetBool(stateMachine.Player.AnimationData.StunParameterHash, true);
        anim.SetLayerWeight(stunLayerIndex, 1);
    }

    public void Exit()
    {
        var anim = stateMachine.Player.Animator;
        anim.SetBool(stateMachine.Player.AnimationData.StunParameterHash, false);
        anim.SetLayerWeight(stunLayerIndex, 0);

        stateMachine.Player.Ability.EndStun();
    }

    public void HandleInput() { }

    public void LogicUpdate()
    {
        Debug.Log(duration);

        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Exit();
        }
    }

    public void PhysicsUpdate() { }
}
