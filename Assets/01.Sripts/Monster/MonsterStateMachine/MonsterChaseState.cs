using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : MonsterBaseState
{
    public MonsterChaseState(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        MoveTo(stateMachine.Monster.PlayerTarget.position);
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
    }
}
