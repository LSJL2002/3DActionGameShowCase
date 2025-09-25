using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class PlayerKnockbackState : Istate
{
    private readonly PlayerStateMachine stateMachine;

    private readonly float layerBlendSpeed;
    private readonly int knockLayerIndex;

    private Vector3 knockDirection;
    private float knockForce;
    private float duration;
    private float elapsed;

    private bool targetLayerOn = false;
    private float currentLayerWeight = 0f;

    public PlayerKnockbackState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.layerBlendSpeed = layerBlendSpeed;

        knockLayerIndex = stateMachine.Player.Animator.GetLayerIndex("Overall/Toggle_HitStopLayer");
    }

    /// <summary>
    /// 외부(몬스터)에서 넉백 정보를 세팅
    /// </summary>
    public void Setup(Vector3 direction, float force, float time)
    {
        knockDirection = direction;
        knockForce = force;
        duration = time;
        elapsed = 0f;
    }

    public void Enter()
    {
        targetLayerOn = true;
        // 애니메이션 트리거
        var anim = stateMachine.Player.Animator;
        anim.SetTrigger(stateMachine.Player.AnimationData.KnockbackParameterHash);

        // 캐릭터를 넉백 방향으로 바라보게 회전 고정
        stateMachine.Player.transform.forward = knockDirection;

        // 제어 불가 상태 → 이동/회전 입력 무시
        stateMachine.IsKnockback = true;
    }

    public void Exit()
    {
        targetLayerOn = false;
        stateMachine.IsKnockback = false;
    }

    public void HandleInput() { /* 넉백 중 입력 무시 */ }

    public void LogicUpdate()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Exit();
            // 넉백 종료 → Idle로 복귀
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        UpdateLayerWeight();
    }

    public void PhysicsUpdate()
    {
        // 넉백 방향으로 밀기
        stateMachine.Player.Controller.Move(knockDirection * knockForce * Time.deltaTime);
    }

    private void UpdateLayerWeight()
    {
        float target = targetLayerOn ? 1f : 0f;
        currentLayerWeight = Mathf.MoveTowards(
            currentLayerWeight,
            target,
            layerBlendSpeed * Time.deltaTime
        );

        stateMachine.Player.Animator.SetLayerWeight(knockLayerIndex, currentLayerWeight);
    }
}
