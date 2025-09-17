using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBaseState : Istate
{
    protected readonly MonsterStateMachine stateMachine;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Update()
    {
        ApplyForcesOnly();
    }
    public virtual void OnAttackHit() { }

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

    //Movement Helpers (NavMesh)

    protected void MoveTo(Vector3 destination)
    {
        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        var forceReceiver = stateMachine.Monster.GetComponent<ForceReceiver>();
        var agent = stateMachine.Monster.Agent;
        if (cc == null || forceReceiver == null || agent == null) return;

        // Make sure agent is active
        if (!agent.isActiveAndEnabled) return;

        // Set the destination on the agent (so it calculates the path)
        agent.SetDestination(destination);

        // Get the agent's desired velocity (what direction it wants to go)
        Vector3 desiredVelocity = agent.desiredVelocity;

        // AI movement from agent
        Vector3 aiMove = desiredVelocity.normalized * stateMachine.MovementSpeed * Time.deltaTime;

        // Combine with forces (gravity, knockback)
        Vector3 totalMove = aiMove + forceReceiver.Movement;

        // Move using CharacterController
        cc.Move(totalMove);

        // Smooth rotation toward movement direction
        Vector3 lookDir = new Vector3(desiredVelocity.x, 0, desiredVelocity.z);
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            stateMachine.Monster.transform.rotation = Quaternion.Slerp(
                stateMachine.Monster.transform.rotation,
                targetRot,
                Time.deltaTime * 10f
            );
        }
    }


    protected void StopMoving()
    {
        var agent = stateMachine.Monster.Agent;
        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        var forceReceiver = stateMachine.Monster.GetComponent<ForceReceiver>();

        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        // Still apply gravity/knockback
        if (cc != null && forceReceiver != null)
        {
            cc.Move(forceReceiver.Movement * Time.deltaTime);
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

        cc.Move(forceReceiver.Movement * Time.deltaTime);
    }
}
