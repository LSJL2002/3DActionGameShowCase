using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }

    public GameObject Target { get; private set; }

    // All States
    public MonsterIdleState MonsterIdleState { get; }
    public MonsterChaseState MonsterChaseState { get; }
    public MonsterBaseAttack MonsterBaseAttack { get; private set; }

    //SmileToiletSkill
    public SmileToiletSlamState SmileToiletSlamState { get; private set; }
    public SmileToiletSmashState SmileToiletSmashState { get; private set; }
    public SmileToiletChargeState SmileToiletChargeState { get; private set; }


    private MonsterAIEvents aiEvents;

    public bool isAttacking = false;

    // Track the current state explicitly
    private Istate currentStateInternal;
    public Istate CurrentState => currentStateInternal;

    public MonsterStateMachine(BaseMonster monster)
    {
        Monster = monster;
        MovementSpeedModifier = 1f;

        MonsterIdleState = new MonsterIdleState(this);
        MonsterChaseState = new MonsterChaseState(this);
        MonsterBaseAttack = new MonsterBaseAttack(this);

        if (monster is ToiletMonster)
        {
            var slamSkill = monster.Stats.GetSkill("SmileMachine_Slam");
            SmileToiletSlamState = new SmileToiletSlamState(this, slamSkill);
            var smashSkill = monster.Stats.GetSkill("SmileMachine_Smash");
            SmileToiletSmashState = new SmileToiletSmashState(this, smashSkill);
            var chargeSkill = monster.Stats.GetSkill("SmileMachine_Charge");
            SmileToiletChargeState = new SmileToiletChargeState(this, chargeSkill);
        }

        aiEvents = monster.GetComponent<MonsterAIEvents>();
        if (aiEvents == null)
        {
            aiEvents = monster.gameObject.AddComponent<MonsterAIEvents>();
        }

        ChangeState(MonsterIdleState);
    }

    // Override ChangeState to track the current state
    public new void ChangeState(Istate newState)
    {
        base.ChangeState(newState);
        currentStateInternal = newState;
    }

    public void EnableAIEvents()
    {
        if (aiEvents == null) return;

        aiEvents.OnPlayerDetected += HandlePlayerDetected;
        aiEvents.RestingPhase += HandlePlayerLost;
        aiEvents.OnInAttackRange += HandlePlayerInAttackRange;
    }

    public void DisableAIEvents()
    {
        if (aiEvents == null) return;

        aiEvents.OnPlayerDetected -= HandlePlayerDetected;
        aiEvents.RestingPhase -= HandlePlayerLost;
        aiEvents.OnInAttackRange -= HandlePlayerInAttackRange;
    }

    private void HandlePlayerDetected()
    {
        ChangeState(MonsterChaseState);
    }

    private void HandlePlayerLost()
    {
        ChangeState(MonsterIdleState);
    }

    private void HandlePlayerInAttackRange()
    {
        // Only attack if currently in Idle state
        if (!isAttacking && CurrentState == MonsterIdleState)
        {
            isAttacking = true;
            if (Monster is ToiletMonster toilet)
            {
                toilet.PickPatternById(5); 
            }
        }
    }

    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
