using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public abstract class PlayerBaseState : Istate
{
    protected PlayerStateMachine sm;
    public PlayerBaseState(PlayerStateMachine sm) => this.sm = sm;

    // 공통 설정
    public virtual bool AllowRotation => true;
    public virtual bool AllowMovement => true;

    // 진입/종료
    public virtual void Enter() { }
    public virtual void Exit() { }
    protected void StartAnimation(int animatorHash) => sm.Player.Animator.SetBool(animatorHash, true);
    protected void StopAnimation(int animatorHash) => sm.Player.Animator.SetBool(animatorHash, false);

    public virtual void HandleInput()
    {
        // Animator에 입력값 전달 (공통 처리)
        sm.Player.Animator.SetFloat(
            sm.Player.AnimationData.HorizontalHash,
            sm.MovementInput.x
        );
        sm.Player.Animator.SetFloat(
            sm.Player.AnimationData.VerticalHash,
            sm.MovementInput.y
        );
    }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() => MoveCharacter();


    // ==================== 플레이어 이동 =======================
    private void MoveCharacter()
    {
        if (!AllowRotation && !AllowMovement)
            return; // 회전/이동 모두 막기

        Vector3 moveDir = GetMovementDir();

        // 캐릭터 회전
        if (AllowRotation && moveDir.sqrMagnitude > 0.01f) // 회전 제어
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            sm.Player.transform.rotation = Quaternion.Slerp(
                sm.Player.transform.rotation,
                targetRot,
                Time.deltaTime * sm.RotationDamping
            );
        }
        // 이동 처리
        if (AllowMovement)
        {
            Vector3 move = Vector3.zero;

            if (sm.Player.Animator.applyRootMotion)
            {
                move = sm.Player.Animator.deltaPosition + sm.Player.ForceReceiver.Movement * Time.deltaTime;
            }
            else
            {
                float moveSpeed = sm.MovementSpeed * sm.MovementSpeedModifier;
                move = moveDir * moveSpeed + sm.Player.ForceReceiver.Movement;
            }

            sm.Player.Controller.Move(move * Time.deltaTime);
        }
    }

    protected Vector3 GetMovementDir()
    {
        Vector3 forward = sm.Player._camera.MainCamera.forward;
        Vector3 right = sm.Player._camera.MainCamera.right;
        
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        return forward * sm.MovementInput.y + right * sm.MovementInput.x;
    }


    // ====================== 에니메이션 특정 태그 =============================
    protected float GetNormalizeTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // 애니메이션 전환(Transition) 중이고, 다음 상태가 지정한 tag면 → 다음 애니메이션의 진행도 반환
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;
        // 전환 중이 아니고, 현재 상태가 지정한 tag면 → 현재 애니메이션의 진행도 반환
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            return currentInfo.normalizedTime;
        // 태그랑 맞는 애니메이션이 없다면 0 반환
        else
            return 0f;
    }
}