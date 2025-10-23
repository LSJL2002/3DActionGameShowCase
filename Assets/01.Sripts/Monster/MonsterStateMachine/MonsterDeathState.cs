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
            stateMachine.Monster.Agent.updateRotation = false;

        var cc = stateMachine.Monster.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Death));
        stateMachine.Monster.IsDead = true;

        int defaultLayer = LayerMask.NameToLayer("Default");
        SetLayerRecursively(stateMachine.Monster.gameObject, defaultLayer);
    }
    
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    public override void Exit()
    {
        
    }

}
