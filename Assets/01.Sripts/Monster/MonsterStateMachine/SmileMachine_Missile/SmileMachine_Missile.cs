using UnityEngine;

public class SmileMachine_Missile : BaseMonster
{
    public Collider baseAttackCollider;
    [Header("Missile Effect")]
    public GameObject missile;
    public Transform firepoint;

    [Header("Gernade Effect")]
    public GameObject gernade;
    public Transform firepointGernade;

    protected override void Awake()
    {
        base.Awake();
    }

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
        if (state is SmileToiletSlamState) return Stats.GetSkill("SmileMachine_Slam").range / 2f;
        if (state is SmileToiletSmashState) return Stats.GetSkill("SmileMachine_Smash").range / 2f;
        if (state is SmileToiletChargeState) return Stats.GetSkill("SmileMachine_Charge").range;
        return Stats.AttackRange;
    }
    
}
