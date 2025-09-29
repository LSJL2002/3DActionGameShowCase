using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderTractor : BaseMonster
{
    public Collider baseAttackCollider;
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            default: return null;
        }
    }

    protected override float GetSkillRangeFromState(MonsterBaseState state)
    {
        switch (state)
        {
            default:
                return Stats.AttackRange;
        }
    }

}
