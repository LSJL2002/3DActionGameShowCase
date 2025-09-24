using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class PlayerKnockbackState : PlayerBaseState
{
    private Vector3 knockDirection;
    private float knockForce;
    private float duration;
    private float elapsed;


    public PlayerKnockbackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public void Setup(Vector3 direction, float force, float time)
    {
        knockDirection = direction;
        knockForce = force;
        duration = time;
        elapsed = 0f;
    }

    public override void Enter()
    {
        // 애니메이션 트리거
        stateMachine.Player.Animator.SetTrigger("Knockback");

        // 플레이어 회전은 넉백 방향으로 고정
        stateMachine.Player.transform.forward = knockDirection;
    }

    public override void LogicUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        stateMachine.Player.Controller.Move(knockDirection * knockForce * Time.deltaTime);
    }
}
