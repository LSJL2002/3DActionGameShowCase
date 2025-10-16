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
        StartAnimation(sm.Player.AnimationData.GroundBoolHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.GroundBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        Vector3 inputDir = GetMovementDir();
        bool hasInput = inputDir.sqrMagnitude > 0.0001f;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (sm.MovementInput == Vector2.zero) return;

        sm.ChangeState(sm.IdleState);

        base.OnMoveCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        sm.ChangeState(sm.JumpState);
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        base.OnAttackStarted(context);
        if (!sm.IsAttacking)
            sm.ChangeState(sm.AttackState);
    }

    protected override void OnSkillStarted(InputAction.CallbackContext context)
    {
        base.OnSkillStarted(context);
        if (sm.Player.Stats.UseSkill())   // 성공했을 때만
            sm.ChangeState(sm.SkillState);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        if(sm.Player.Stats.UseEvade())
            sm.ChangeState(sm.DodgeState);
    }
}