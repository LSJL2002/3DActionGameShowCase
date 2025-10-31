using UnityEngine;

public class SmileMachine_UseMissile : BaseMonster
{
    public LineRenderer lineRender;
    [Header("Missile Effect")]
    public GameObject missile;
    public GameObject missileEffect;
    public Transform firepoint;

    [Header("Gernade Effect")]
    public GameObject gernade;
    public Transform firepointGernade;

    //Funnel Event
    private bool hasSent90HPEvent = false;
    private bool hasSent60HPEvent = false;
    private bool hasSent30HPEvent = false;
    private bool hasSent10HPEvent = false;
    private bool hasDied = false;

    protected override void Awake()
    {
        base.Awake();
        if (lineRender != null)
        {
            lineRender.positionCount = 2;
            lineRender.enabled = false;
        }
    }

    protected override MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1: return stateMachine.SmileToiletSmashState;
            case States.Skill2: return stateMachine.SmileToiletSlamState;
            case States.Skill3: return stateMachine.SmileToiletChargeState;
            case States.Skill4: return stateMachine.SmileMachine_Missile;
            case States.Skill5: return stateMachine.SmileMachine_Gernade;
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
        if (state is SmileMachine_Missile) return Stats.GetSkill("SmileMachine_Missile").skillUseRange;
        if (state is SmileMachine_Gernade) return Stats.GetSkill("SmileMachine_Grenade").skillUseRange;
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
            AnalyticsManager.SendFunnelStep("42");
            Debug.Log("Analytics value 42 sent.");
        }

        // 60%
        if (!hasSent60HPEvent && hpPercent <= 60f)
        {
            hasSent60HPEvent = true;
            AnalyticsManager.SendFunnelStep("43");
            Debug.Log("Analytics value 43 sent.");
        }

        // 30%
        if (!hasSent30HPEvent && hpPercent <= 30f)
        {
            hasSent30HPEvent = true;
            AnalyticsManager.SendFunnelStep("44");
            Debug.Log("Analytics value 44 sent.");
        }

        // 10%
        if (!hasSent10HPEvent && hpPercent <= 10f)
        {
            hasSent10HPEvent = true;
            AnalyticsManager.SendFunnelStep("45");
            Debug.Log("Analytics value 45 sent.");
        }

        // 0%
        if (!hasDied && hpPercent <= 0f)
        {
            hasDied = true;
            AnalyticsManager.SendFunnelStep("46");
            Debug.Log("Analytics value 46 sent");
        }
    }
}
