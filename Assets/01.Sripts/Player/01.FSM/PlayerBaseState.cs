using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : Istate
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }
    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled += OnMovementCanceled;
        input.PlayerActions.Run.started += OnRunStarted;
        input.PlayerActions.Run.canceled += OnRunCanceled;
        input.PlayerActions.Attack.performed += OnAttackPerformed;
        input.PlayerActions.Attack.canceled += OnAttackCanceled;
        //input.PlayerActions.Jump.started += OnJumpStarted;

    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled -= OnMovementCanceled;
        input.PlayerActions.Run.started -= OnRunStarted;
        input.PlayerActions.Run.canceled -= OnRunCanceled;
        input.PlayerActions.Attack.performed -= OnAttackPerformed;
        input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        //input.PlayerActions.Jump.started -= OnJumpStarted;

    }

    public virtual void HandleInput() => ReadMovementInput();
    public virtual void PhysicsUpdate() => MoveCharacter();
    public virtual void LogicUpdate() => UpdateAnimatorMovementSpeed();


    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) { }

    protected virtual void OnRunStarted(InputAction.CallbackContext context) { }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context) { }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context) { }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
         => stateMachine.IsAttacking = true;

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
        => stateMachine.IsAttacking = false;


    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    private void UpdateAnimatorMovementSpeed()
    {
        float speed = stateMachine.MovementInput.magnitude * stateMachine.MovementSpeedModifier;
        stateMachine.Player.Animator.SetFloat(
            stateMachine.Player.AnimationData.MovementSpeedParameterHash,
            speed
        );
    }


    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    private void MoveCharacter()
    {
        Vector3 moveDir = GetMovementDir();

        //캐릭터 회전
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            stateMachine.Player.transform.rotation = Quaternion.Slerp(
                stateMachine.Player.transform.rotation,
                targetRot,
                Time.deltaTime * stateMachine.RotationDamping
            );
        }

        // 이동 처리
        if (stateMachine.Player.Animator.applyRootMotion)
        {
            // 루트모션 사용 시 Animator.deltaPosition 적용
            Vector3 rootMove = stateMachine.Player.Animator.deltaPosition;
            rootMove += stateMachine.Player.ForceReceiver.Movement * Time.deltaTime;
            stateMachine.Player.Controller.Move(rootMove);
        }
        else
        {
            // 루트모션 꺼져있으면 기존 방식
            float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
            Vector3 move = moveDir * moveSpeed + stateMachine.Player.ForceReceiver.Movement;
            stateMachine.Player.Controller.Move(move * Time.deltaTime);
        }
    }

    protected Vector3 GetMovementDir()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }


    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
    }

    protected float GetNormalizeTime(Animator animator, string tag) //에니메이션 특정 태그
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}