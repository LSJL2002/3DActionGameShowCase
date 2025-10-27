using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SpiderTractor_UseGrenade : BaseMonster
{
    [Header("StampSkill")]
    public GameObject stampEffect;
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            case States.Skill1: return stateMachine.SpiderMachine_AttackStamp;
            case States.TurnLeft: return stateMachine.SpiderMachine_TurnLeft;
            default: return null;
        }
    }

    protected override float GetSkillRangeFromState(MonsterBaseState state)
    {
        if (state is SpiderMachine_AttackStamp)
            return Stats.GetSkill("SpiderMachine_AttackStamp").skillUseRange;
        return Stats.AttackRange;
    }
}
