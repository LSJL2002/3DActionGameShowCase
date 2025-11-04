using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    private bool hasSent90HPEvent = false;
    private bool hasSent60HPEvent = false;
    private bool hasSent30HPEvent = false;
    private bool hasSent10HPEvent = false;
    private bool hasDied = false;
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
        if (state is SmileToiletSlamState)
            return Stats.GetSkill("SmileMachine_Slam").range / 2f;
        if (state is SmileToiletSmashState)
            return Stats.GetSkill("SmileMachine_Smash").range / 2f;
        if (state is SmileToiletChargeState)
            return Stats.GetSkill("SmileMachine_Charge").skillUseRange;
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
            AnalyticsManager.SendFunnelStep("12");
            Debug.Log("ToiletMonster reached 90% HP — Analytics value 12 sent.");
        }

        // 60%
        if (!hasSent60HPEvent && hpPercent <= 60f)
        {
            hasSent60HPEvent = true;
            AnalyticsManager.SendFunnelStep("13");
            Debug.Log("ToiletMonster reached 60% HP — Analytics value 13 sent.");
        }

        // 30%
        if (!hasSent30HPEvent && hpPercent <= 30f)
        {
            hasSent30HPEvent = true;
            AnalyticsManager.SendFunnelStep("14");
            Debug.Log("ToiletMonster reached 30% HP — Analytics value 14 sent.");
        }

        // 10%
        if (!hasSent10HPEvent && hpPercent <= 10f)
        {
            hasSent10HPEvent = true;
            AnalyticsManager.SendFunnelStep("15");
            Debug.Log("ToiletMonster reached 10% HP — Analytics value 15 sent.");
        }

        // 0%
        if (!hasDied && hpPercent <= 0f)
        {
            hasDied = true;
            AnalyticsManager.SendFunnelStep("16");
            Debug.Log("ToiletMonster reached 0%hp and died - Analytics value 16 sent");
        }
    }
}
