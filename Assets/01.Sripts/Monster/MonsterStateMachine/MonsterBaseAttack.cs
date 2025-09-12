using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseAttack : MonsterBaseState
{
    public MonsterBaseAttack(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        StopMoving();
        PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.BaseAttack));
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
