using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    private bool isWalking;

    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);

        isWalking = false;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
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


        Rotate(inputDir);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(!stateMachine.Player.Controller.isGrounded 
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return;

        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.ComboAttackState);
    }

    private void Rotate(Vector3 dir)
    {
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            stateMachine.Player.transform.rotation = Quaternion.Slerp(
                stateMachine.Player.transform.rotation,
                targetRot,
                Time.deltaTime * stateMachine.RotationDamping
            );
        }
    }
}
