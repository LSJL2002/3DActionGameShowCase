using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }

    public GameObject Target { get; private set; }

    // 모든 State 
    public MonsterIdleState MonsterIdleState { get; }
    public MonsterSkillOneState MonsterSkillOneState { get; set; }
    public MonsterChaseState MonsterChaseState { get; }

    private MonsterAIEvents aiEvents;

    public bool isAttacking = false;

    public MonsterStateMachine(BaseMonster monster)
    {
        Monster = monster;

        MovementSpeedModifier = 1f;

        MonsterIdleState = new MonsterIdleState(this);
        MonsterChaseState = new MonsterChaseState(this);
        MonsterSkillOneState = new MonsterSkillOneState(this);


        aiEvents = monster.GetComponent<MonsterAIEvents>();
        if (aiEvents == null)
        {
            aiEvents = monster.gameObject.AddComponent<MonsterAIEvents>();
        }
    }
    public void EnableAIEvents()
    {
        if (aiEvents == null) return;

        aiEvents.OnPlayerDetected += HandlePlayerDetected;
        aiEvents.OnInAttackRange += HandlePlayerInAttackRange;
        aiEvents.RestingPhase += HandleRestingPhase;
    }

    public void DisableAIEvents()
    {
        if (aiEvents == null) return;

        aiEvents.OnPlayerDetected -= HandlePlayerDetected;
        aiEvents.OnInAttackRange -= HandlePlayerInAttackRange;
        aiEvents.RestingPhase -= HandleRestingPhase;
    }

    private void HandlePlayerDetected()
    {
        ChangeState(MonsterChaseState);
    }

    private void HandlePlayerInAttackRange()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            ChangeState(MonsterSkillOneState);
        }
    }

    private void HandleRestingPhase()
    {
        ChangeState(MonsterIdleState);
    }
    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
