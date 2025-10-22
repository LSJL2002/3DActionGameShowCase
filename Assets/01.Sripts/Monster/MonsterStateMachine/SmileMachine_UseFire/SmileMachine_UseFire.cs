using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachine_UseFire : BaseMonster
{
    [Header("FlameThrower Effect")]
    public GameObject flameThrowerEffect;
    public GameObject firePoint;

    [Header("Fireball Effects")]
    public GameObject fireball;
    public GameObject groundFire;

    protected override void Awake()
    {
        base.Awake();
        if (flameThrowerEffect != null)
        {
            flameThrowerEffect.SetActive(false);
        }
    }
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1: return stateMachine.SmileToiletSmashState;
            case States.Skill2: return stateMachine.SmileToiletSlamState;
            case States.Skill3: return stateMachine.SmileToiletChargeState;
            case States.Skill4: return stateMachine.SmileMachineFire;
            case States.Skill5: return stateMachine.SmileMachine_FireShoot;
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            default: return null;
        }
    }

    protected override float GetSkillRangeFromState(MonsterBaseState state)
    {
        if (state is SmileToiletSlamState) return Stats.GetSkill("SmileMachine_Slam").range / 2f;
        if (state is SmileToiletSmashState) return Stats.GetSkill("SmileMachine_Smash").range / 2f;
        if (state is SmileToiletChargeState) return Stats.GetSkill("SmileMachine_Charge").range;
        if (state is SmileMachineFire) return Stats.GetSkill("SmileMachine_Fire").range;
        if (state is SmileMachine_FireShoot) return Stats.GetSkill("SmileMachine_FireShoot").skillUseRange;

        return Stats.AttackRange;
    }
}
