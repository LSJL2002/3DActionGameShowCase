using System.Collections;
using System.Collections.Generic;
using System.Data;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    public MonsterDeathState(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        StopMoving();
        stateMachine.isAttacking = false;
        stateMachine.DisableAIEvents();
        stateMachine.DisableAIProcessing();
        stateMachine.Monster.ClearAOEs();

        if (stateMachine.Monster.Agent != null)
        {
            stateMachine.Monster.Agent.isStopped = true;
            stateMachine.Monster.Agent.ResetPath();
            stateMachine.Monster.Agent.updateRotation = false;
            stateMachine.Monster.Agent.enabled = false;
        }
        
        if (stateMachine.Monster.Agent != null)
            stateMachine.Monster.Agent.updateRotation = false;

        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Death));
        stateMachine.Monster.IsDead = true;
    }


    public override void Exit()
    {
        
    }

}
