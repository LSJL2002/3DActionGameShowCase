using System.Collections;
using System.Collections.Generic;
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
        stateMachine.Monster.ClearAOEs();
        PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Death));
        BattleManager.Instance.HandleMonsterDie();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        
    }

}
