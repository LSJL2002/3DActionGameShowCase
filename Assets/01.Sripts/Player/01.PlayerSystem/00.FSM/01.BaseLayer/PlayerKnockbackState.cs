using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerBaseState
{
    private Vector3 knockDirection;
    private float knockForce;
    private float duration;
    private float elapsed;

    public PlayerKnockbackState(PlayerStateMachine sm) : base(sm) { }


    /// <summary>
    /// 외부(몬스터)에서 넉백 정보를 세팅
    /// </summary>
    public void Setup(Vector3 direction, float force, float time)
    {
        knockDirection = direction;
        knockForce = force;
        duration = time;
        // 이미 넉백 중이라면 → 다시 시작하도록 시간 초기화
        elapsed = 0f;
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.KnockbackParameterHash);
        int knockLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
        sm.Player.Animator.SetLayerWeight(knockLayerIndex, 1);

        // 넉백 힘 바로 적용
        sm.Player.ForceReceiver.AddForce(knockDirection * knockForce);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.KnockbackParameterHash);
        int knockLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
        sm.Player.Animator.SetLayerWeight(knockLayerIndex, 0);

        sm.Player.Ability.EndKnockback();
    }

    public override void HandleInput() { /* 넉백 중 입력 무시 */ }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        elapsed += Time.deltaTime;

        // 넉백 중 맞은 몬스터 바라보기 대신 넉백 반대 방향 바라보기
        if (knockDirection.sqrMagnitude > 0.01f)
        {
            Vector3 lookDir = -knockDirection; // 넉백 방향 반대로
            lookDir.y = 0f;                    // 수직 회전 제거
            sm.Player.transform.forward = lookDir.normalized;
        }

        if (elapsed >= duration)
        {
            Exit();
        }
    }
}