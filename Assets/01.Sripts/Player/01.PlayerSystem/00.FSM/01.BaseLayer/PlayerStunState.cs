using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : PlayerBaseState
{
    private float duration;
    private float elapsed;

    public PlayerStunState(PlayerStateMachine sm) : base(sm) { }


    /// <summary>
    /// 외부(몬스터)에서 스턴 시간 설정
    /// </summary>
    public void Setup(float time)
    {
        duration = time;
        // 이미 넉백 중이라면 → 다시 시작하도록 시간 초기화
        elapsed = 0f;
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.StunParameterHash);
        int stunLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
        sm.Player.Animator.SetLayerWeight(stunLayerIndex, 1);
        sm.Player.Motor.AllowMovement = false;
    }

    public override void Exit()
    {
        base.Exit();
        StartAnimation(sm.Player.AnimationData.StunParameterHash);
        int stunLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
        sm.Player.Animator.SetLayerWeight(stunLayerIndex, 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            sm.Player.Ability.EndStun();
        }
    }
}