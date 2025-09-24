using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class PlayerStunState : PlayerBaseState
{
    private float duration;
    private float elapsed;


    public PlayerStunState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public void Setup(float time)
    {
        duration = time;
        elapsed = 0f;
    }

    public override void Enter()
    {
        // 애니메이션 트리거
        stateMachine.Player.Animator.SetTrigger("Stun");
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
        // 이동 금지 → ForceReceiver나 Controller 이동 없음
    }
}
