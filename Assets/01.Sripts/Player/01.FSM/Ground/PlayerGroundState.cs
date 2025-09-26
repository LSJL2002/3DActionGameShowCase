using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine sm) : base(sm) { }



    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundBoolHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (stateMachine.IsAttacking)
        {
            OnAttack();
            return;
        }

        Vector3 inputDir = GetMovementDir();
        bool hasInput = inputDir.sqrMagnitude > 0.0001f;

        //Walk상태 무한재진입함 -> 진입은 Idle에서 관리
        //if (hasInput && AllowRotation && AllowMovement)
        //{
        //    stateMachine.ChangeState(stateMachine.WalkState);
        //    return;
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return;

        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMoveCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.AttackState);
    }

    protected override void OnHeavyAttackStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.SkillState);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.DodgeState);
    }
}
