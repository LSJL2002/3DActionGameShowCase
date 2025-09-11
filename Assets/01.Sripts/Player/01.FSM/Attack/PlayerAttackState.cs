using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private readonly int attackHash = Animator.StringToHash("Attack");
    private readonly float dodgeCancelThreshold = 0.3f; // 공격 애니메이션 30% 이후부터 회피 가능

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 공격 끝나면 Idle로 돌아가기
        if (GetNormalizeTime(stateMachine.Player.Animator, "Attack") >= 1f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.DodgeState);
    }
}