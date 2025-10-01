using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (stateMachine.Player.Stats.UseSkill())   // 성공했을 때만
            stateMachine.ChangeState(stateMachine.SkillState);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        if(stateMachine.Player.Stats.UseEvade())
            stateMachine.ChangeState(stateMachine.DodgeState);
    }
}
