using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }
    public MonsterAIEvents AIEvents => aiEvents;

    public MonsterDeathState MonsterDeathState { get; }
    public MonsterIdleState MonsterIdleState { get; }
    public MonsterChaseState MonsterChaseState { get; }
    public MonsterBaseAttack MonsterBaseAttack { get; private set; }
    public MonsterBaseAttackAlt MonsterBaseAttackAlt { get; private set; }

    public SmileToiletSlamState SmileToiletSlamState { get; private set; }
    public SmileToiletSmashState SmileToiletSmashState { get; private set; }
    public SmileToiletChargeState SmileToiletChargeState { get; private set; }

    private MonsterAIEvents aiEvents;
    public bool isAttacking = false;

    private Istate currentStateInternal;
    public Istate CurrentState => currentStateInternal;

    public MonsterStateMachine(BaseMonster monster)
    {
        Monster = monster;
        MovementSpeedModifier = 1f;

        MonsterIdleState = new MonsterIdleState(this);
        MonsterChaseState = new MonsterChaseState(this);
        MonsterBaseAttack = new MonsterBaseAttack(this);
        MonsterDeathState = new MonsterDeathState(this);
        MonsterBaseAttackAlt = new MonsterBaseAttackAlt(this, MonsterAnimationData.MonsterAnimationType.BaseAttack2); // new animation

        if (monster is ToiletMonster)
        {
            var slamSkill = monster.Stats.GetSkill("SmileMachine_Slam");
            if (slamSkill == null)
            {
                Debug.LogError($"Skill 'SmileMachine_Slam' not found! Monster has {monster.Stats.MonsterSkills.Count} skills:");
                foreach (var s in monster.Stats.MonsterSkills)
                    Debug.Log($" - {s.skillName}");
            }
            SmileToiletSlamState = new SmileToiletSlamState(this, slamSkill);
            var smashSkill = monster.Stats.GetSkill("SmileMachine_Smash");
            SmileToiletSmashState = new SmileToiletSmashState(this, smashSkill);
            var chargeSkill = monster.Stats.GetSkill("SmileMachine_Charge");
            SmileToiletChargeState = new SmileToiletChargeState(this, chargeSkill);
        }

        aiEvents = monster.GetComponent<MonsterAIEvents>() ?? monster.gameObject.AddComponent<MonsterAIEvents>();
        aiEvents.SetStateMachine(this);

        EnableAIEvents();

        ChangeState(MonsterIdleState);
    }

    public new void ChangeState(Istate newState)
    {
        base.ChangeState(newState);
        currentStateInternal = newState;
    }

    public void EnableAIEvents()
    {
        aiEvents.OnPlayerDetected += HandleChase;
        aiEvents.RestingPhase += HandleIdle;
        aiEvents.OnInAttackRange += HandlePlayerInAttackRange;
    }

    public void DisableAIEvents()
    {
        aiEvents.OnPlayerDetected -= HandleChase;
        aiEvents.RestingPhase -= HandleIdle;
        aiEvents.OnInAttackRange -= HandlePlayerInAttackRange;
    }

    private void HandleChase()
    {
        if (!isAttacking)
        {
            ChangeState(MonsterChaseState);
        }
    }

    private void HandleIdle()
    {
        if (!isAttacking)
        {
            ChangeState(MonsterIdleState);
        }
    }



    private void HandlePlayerInAttackRange()
    {
        if (!isAttacking && CurrentState == MonsterIdleState)
        {
            isAttacking = true;
            if (Monster is ToiletMonster toilet)
            {
                toilet.PickPatternByCondition();
            }
        }
    }
    //Enable or Disable the AI events
    public void DisableAIProcessing()
    {
        aiEvents?.Disable();
    }

    public void EnableAIProcessing()
    {
        aiEvents?.Enable();
    }

    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
