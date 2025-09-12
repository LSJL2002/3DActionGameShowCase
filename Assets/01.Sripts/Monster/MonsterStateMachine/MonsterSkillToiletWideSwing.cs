using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MonsterSkillToiletWideSwing : MonsterBaseState
{
    public MonsterSkillToiletWideSwing(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        StopMoving();
        PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Attack));
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        
    }
}
