using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SpiderTractor_UseGrenade : BaseMonster
{
    [Header("StampSkill")]
    public GameObject stampEffect;
    [Header("FangFallSkill")]
    public GameObject fangFallEffect;
    [Header("Bombardment")]
    public GameObject missile;
    public Transform firePointMissile;
    //Funnel Events
    private bool hasSent90HPEvent = false;
    private bool hasSent60HPEvent = false;
    private bool hasSent30HPEvent = false;
    private bool hasSent10HPEvent = false;
    private bool hasDied = false;
    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            case States.TurnLeft: return stateMachine.SpiderMachine_TurnLeft;
            case States.TurnRight: return stateMachine.SpiderMachine_TurnRight;
            case States.Skill1: return stateMachine.SpiderMachine_AttackStamp;
            case States.Skill2: return stateMachine.SpiderMachine_FangFall;
            case States.Skill3: return stateMachine.SpiderMachine_Spin;
            case States.Skill4: return stateMachine.SpiderMachine_Bombardment;
            case States.Skill5: return stateMachine.SpiderMachine_BackJump;
            default: return null;
        }
    }

    protected override float GetSkillRangeFromState(MonsterBaseState state)
    {
        if (state is SpiderMachine_AttackStamp)
            return Stats.GetSkill("SpiderMachine_AttackStamp").skillUseRange;
        if (state is SpiderMachine_FangFall)
            return Stats.GetSkill("SpiderMachine_Fangfall").skillUseRange;
        if (state is SpiderMachine_Spin)
            return Stats.GetSkill("SpiderMachine_Spin").skillUseRange;
        if (state is SpiderMachine_Bombardment)
            return Stats.GetSkill("SpiderMachine_Bombardment").skillUseRange;
        return Stats.AttackRange;
    }

    public void JumpTiming()
    {
        // Call the FangFall state's jump coroutine
        if (stateMachine.CurrentState is SpiderMachine_FangFall fangFallState)
        {
            fangFallState.TriggerJump();
        }
    }
    public void SpinTiming()
    {
        if (stateMachine.CurrentState is SpiderMachine_Spin spinState)
        {
            spinState.TriggerSpin();
        }
    }

    public void StopSpin()
    {
        if (stateMachine.CurrentState is SpiderMachine_Spin spinState)
        {
            spinState.StopSpin();
        }
    }
    public override void OnTakeDamage(int amount)
    {
        base.OnTakeDamage(amount);

        float hpPercent = (Stats.CurrentHP / Stats.maxHp) * 100f;

        // 90%
        if (!hasSent90HPEvent && hpPercent <= 90f)
        {
            hasSent90HPEvent = true;
            AnalyticsManager.SendFunnelStep("52");
            Debug.Log("Analytics value 52 sent.");
        }

        // 60%
        if (!hasSent60HPEvent && hpPercent <= 60f)
        {
            hasSent60HPEvent = true;
            AnalyticsManager.SendFunnelStep("53");
            Debug.Log("Analytics value 53 sent.");
        }

        // 30%
        if (!hasSent30HPEvent && hpPercent <= 30f)
        {
            hasSent30HPEvent = true;
            AnalyticsManager.SendFunnelStep("54");
            Debug.Log("Analytics value 54 sent.");
        }

        // 10%
        if (!hasSent10HPEvent && hpPercent <= 10f)
        {
            hasSent10HPEvent = true;
            AnalyticsManager.SendFunnelStep("55");
            Debug.Log("Analytics value 55 sent.");
        }

        // 0%
        if (!hasDied && hpPercent <= 0f)
        {
            hasDied = true;
            AnalyticsManager.SendFunnelStep("56");
            Debug.Log("Analytics value 56 sent");
        }
    }
}
