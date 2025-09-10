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
    public virtual void PhysicsUpdate() { }
    public virtual void Update() { }

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
        if (stateMachine.Monster.Agent.isActiveAndEnabled)
        {
            stateMachine.Monster.Agent.isStopped = false;
            stateMachine.Monster.Agent.speed = stateMachine.MovementSpeed;
            stateMachine.Monster.Agent.angularSpeed = 200f;

            stateMachine.Monster.Agent.SetDestination(destination);
        }
    }

    protected void StopMoving()
    {
        if (stateMachine.Monster.Agent.isActiveAndEnabled)
        {
            stateMachine.Monster.Agent.isStopped = true;
            stateMachine.Monster.Agent.velocity = Vector3.zero;
        }
    }
    protected bool IsEnemyInDetectionRange()
    {
        if (stateMachine.Monster.EnemeyTarget == null) return false;

        float distSqr = (stateMachine.Monster.EnemeyTarget.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return true;
    }
}
