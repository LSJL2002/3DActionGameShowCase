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
    
    //Funnel Events
    private bool hasSent90HPEvent = false;
    private bool hasSent60HPEvent = false;
    private bool hasSent30HPEvent = false;
    private bool hasSent10HPEvent = false;
    private bool hasDied = false;
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
    public override void OnTakeDamage(int amount)
    {
        base.OnTakeDamage(amount);

        float hpPercent = (Stats.CurrentHP / Stats.maxHp) * 100f;

        // 90%
        if (!hasSent90HPEvent && hpPercent <= 90f)
        {
            hasSent90HPEvent = true;
            AnalyticsManager.SendFunnelStep("32");
            Debug.Log("Analytics value 32 sent.");
        }

        // 60%
        if (!hasSent60HPEvent && hpPercent <= 60f)
        {
            hasSent60HPEvent = true;
            AnalyticsManager.SendFunnelStep("33");
            Debug.Log("Analytics value 33 sent.");
        }

        // 30%
        if (!hasSent30HPEvent && hpPercent <= 30f)
        {
            hasSent30HPEvent = true;
            AnalyticsManager.SendFunnelStep("34");
            Debug.Log("Analytics value 34 sent.");
        }

        // 10%
        if (!hasSent10HPEvent && hpPercent <= 10f)
        {
            hasSent10HPEvent = true;
            AnalyticsManager.SendFunnelStep("35");
            Debug.Log("Analytics value 35 sent.");
        }

        // 0%
        if (!hasDied && hpPercent <= 0f)
        {
            hasDied = true;
            AnalyticsManager.SendFunnelStep("36");
            Debug.Log("Analytics value 36 sent");
        }
    }
}
