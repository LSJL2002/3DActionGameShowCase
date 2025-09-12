using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    public MonsterIdleState(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
    }
    
}
