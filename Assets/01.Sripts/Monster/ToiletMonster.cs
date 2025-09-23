using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    public Collider baseAttackCollider;
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1: return stateMachine.SmileToiletSmashState;
            case States.Skill2: return stateMachine.SmileToiletSlamState;
            case States.Skill3: return stateMachine.SmileToiletChargeState;
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            default: return null;
        }
    }

    protected override float GetSkillRangeFromState(MonsterBaseState state)
    {
        switch (state)
        {
            case SmileToiletSlamState:
                return Stats.GetSkill("SmileMachine_Slam").range / 2f;
            case SmileToiletSmashState:
                return Stats.GetSkill("SmileMachine_Smash").range / 2f;
            case SmileToiletChargeState:
                return Stats.GetSkill("SmileMachine_Charge").range;
            default:
                return Stats.AttackRange;
        }
    }
}
