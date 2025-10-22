using UnityEngine;

public class SmileMachine_UseMissile : BaseMonster
{
    public Collider baseAttackCollider;
    public LineRenderer lineRender;
    [Header("Missile Effect")]
    public GameObject missile;
    public GameObject missileEffect;
    public Transform firepoint;

    [Header("Gernade Effect")]
    public GameObject gernade;
    public Transform firepointGernade;

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
    
}
