using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MonsterBaseState : Istate
{
    protected readonly MonsterStateMachine stateMachine;
    public event Action OnStateFinished;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }


    public virtual void Enter() { }

    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void OnControllerColliderHit(ControllerColliderHit hit) { }
    public virtual void Update()
    {
        ApplyForcesOnly();
    }
    public virtual void OnAttackHit() { }
    public virtual void OnAnimationComplete()
    {
        
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, false);
    }
    protected void PlayTriggerAnimation(int triggerHash)
    {
        stateMachine.Monster.Animator.SetTrigger(triggerHash);
    }
    protected void StateFinished()
    {
        OnStateFinished?.Invoke();
    }

    //Movement Helpers (NavMesh)

    protected void MoveTo(Vector3 destination)
    {
        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        var agent = stateMachine.Monster.Agent;
        if (cc == null || agent == null) return;

        if (agent.isStopped)
            agent.isStopped = false;

        // NavMeshAgent은 이동 경로에만 사용
        agent.SetDestination(destination);
        Vector3 desiredVelocity = agent.desiredVelocity;

        // 찾은 경로와 force을 이용을하여 몬스터를 이동을 하게 한다
        Vector3 move = desiredVelocity.normalized * stateMachine.MovementSpeed * Time.deltaTime;

        cc.Move(move);

        // Rotation하는 방법
        if (desiredVelocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(desiredVelocity.x, 0, desiredVelocity.z));
            stateMachine.Monster.transform.rotation = Quaternion.Slerp(
                stateMachine.Monster.transform.rotation,
                targetRot,
                Time.deltaTime * 10f
            );
        }
    }


    protected void StopMoving()
    {
        if (stateMachine.Monster.IsDead)
        {
            return;
        }
        var agent = stateMachine.Monster.Agent;
        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        var forceReceiver = stateMachine.Monster.GetComponent<ForceReceiver>();

        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (cc != null)
        {
            Vector3 move = forceReceiver != null ? forceReceiver.HorizontalVelocity * Time.deltaTime : Vector3.zero;

            Vector3 top = cc.transform.position + cc.center + Vector3.up * (cc.height / 2 - cc.radius);
            Vector3 bottom = cc.transform.position + cc.center - Vector3.up * (cc.height / 2 - cc.radius);

            Collider[] hits = Physics.OverlapCapsule(top, bottom, cc.radius, LayerMask.GetMask("Player"));

            foreach (var hit in hits)
            {
                Vector3 dir = cc.transform.position - hit.transform.position;
                dir.y = 0f;

                if (dir.sqrMagnitude > 0.0001f)
                {
                    dir.Normalize();
                    move += dir * 0.02f;
                }
            }
            float vertical = move.y; 
            move.y = 0f;

            cc.Move(new Vector3(move.x, 0f, move.z) + Vector3.up * vertical);
        }
    }

    protected bool IsEnemyInDetectionRange()
    {
        if (stateMachine.Monster.PlayerTarget == null) return false;

        float distSqr = (stateMachine.Monster.PlayerTarget.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return distSqr <= stateMachine.Monster.Stats.DetectRange * stateMachine.Monster.Stats.DetectRange;
    }

    protected void ApplyForcesOnly()
    {
        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        var forceReceiver = stateMachine.Monster.GetComponent<ForceReceiver>();
        if (cc == null || forceReceiver == null) return;

        Vector3 move = forceReceiver.HorizontalVelocity * Time.deltaTime;

        if (cc.isGrounded && move.y <= 0f)
        {
            move.y = -0.1f; 
        }

        cc.Move(move);
    }
}
