using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        stateMachine.isAttacking = true;
        StopMoving();
        PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Attack));
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        
    }
}
