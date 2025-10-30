using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachine_UseGun : BaseMonster
{
    public GameObject rifleParticleEffect;
    public GameObject groggyParticleEffect;

    protected override void Awake()
    {
        base.Awake();

        if (rifleParticleEffect != null)
            rifleParticleEffect.SetActive(false);
        if (groggyParticleEffect != null)
            groggyParticleEffect.SetActive(false);
    }
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1: return stateMachine.SmileToiletSmashState;
            case States.Skill2: return stateMachine.SmileToiletSlamState;
            case States.Skill3: return stateMachine.SmileToiletChargeState;
            case States.Skill4: return stateMachine.SmileMachineShootState;
            case States.Skill5: return stateMachine.SmileMachineGroggyShoot;
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            default: return null;
        }
    }

    protected override float GetSkillRangeFromState(MonsterBaseState state)
    {
        if (state is SmileToiletSlamState)
            return Stats.GetSkill("SmileMachine_Slam").range / 2f;
        if (state is SmileToiletSmashState)
            return Stats.GetSkill("SmileMachine_Smash").range / 2f;
        if (state is SmileToiletChargeState)
            return Stats.GetSkill("SmileMachine_Charge").range;
        if (state is SmileMachineShootState)
            return Stats.GetSkill("SmileMachine_Shoot").skillUseRange;
        if (state is SmileMachineGroggyShoot)
            return Stats.GetSkill("SmileMachine_GroggyShoot").skillUseRange;

        return Stats.AttackRange;
    }
}
