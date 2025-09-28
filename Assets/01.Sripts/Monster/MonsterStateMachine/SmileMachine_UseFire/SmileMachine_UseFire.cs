using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachine_UseFire : BaseMonster
{
    public GameObject rifleParticleEffect;
    public Collider baseAttackCollider;

    protected override void Awake()
    {
        base.Awake();

        if (rifleParticleEffect != null)
            rifleParticleEffect.SetActive(false);
    }
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1: return stateMachine.SmileToiletSmashState;
            case States.Skill2: return stateMachine.SmileToiletSlamState;
            case States.Skill3: return stateMachine.SmileToiletChargeState;
            case States.Skill4: return stateMachine.SmileMachineShootState;
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
            case SmileMachineShootState:
                return Stats.GetSkill("SmileMachine_Shoot").range;
            default:
                return Stats.AttackRange;
        }
    }
}
