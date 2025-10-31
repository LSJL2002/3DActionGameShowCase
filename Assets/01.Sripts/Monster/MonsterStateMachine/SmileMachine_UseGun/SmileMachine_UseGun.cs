using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachine_UseGun : BaseMonster
{
    public GameObject rifleParticleEffect;
    public GameObject groggyParticleEffect;

    //Funnel Events
    private bool hasSent90HPEvent = false;
    private bool hasSent60HPEvent = false;
    private bool hasSent30HPEvent = false;
    private bool hasSent10HPEvent = false;
    private bool hasDied = false;

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
    public override void OnTakeDamage(int amount)
    {
        base.OnTakeDamage(amount);

        float hpPercent = (Stats.CurrentHP / Stats.maxHp) * 100f;

        // 90%
        if (!hasSent90HPEvent && hpPercent <= 90f)
        {
            hasSent90HPEvent = true;
            AnalyticsManager.SendFunnelStep("22");
            Debug.Log("Analytics value 22 sent.");
        }

        // 60%
        if (!hasSent60HPEvent && hpPercent <= 60f)
        {
            hasSent60HPEvent = true;
            AnalyticsManager.SendFunnelStep("23");
            Debug.Log("Analytics value 23 sent.");
        }

        // 30%
        if (!hasSent30HPEvent && hpPercent <= 30f)
        {
            hasSent30HPEvent = true;
            AnalyticsManager.SendFunnelStep("24");
            Debug.Log("Analytics value 24 sent.");
        }

        // 10%
        if (!hasSent10HPEvent && hpPercent <= 10f)
        {
            hasSent10HPEvent = true;
            AnalyticsManager.SendFunnelStep("25");
            Debug.Log("Analytics value 25 sent.");
        }

        // 0%
        if (!hasDied && hpPercent <= 0f)
        {
            hasDied = true;
            AnalyticsManager.SendFunnelStep("26");
            Debug.Log("Analytics value 26 sent");
        }
    }
}
