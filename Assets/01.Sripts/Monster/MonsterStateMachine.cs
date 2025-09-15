using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }

    public GameObject Target { get; private set; }

    // 모든 State 
    public MonsterIdleState MonsterIdleState { get; }
    public MonsterChaseState MonsterChaseState { get; }
    public MonsterBaseAttack MonsterBaseAttack { get; private set; }
    public MonsterCenterSkillAttack MonsterCenterSkillAttack { get; private set; }

    // Toilet Monster Skill Sets

    private MonsterAIEvents aiEvents;

    public bool isAttacking = false;

    public MonsterStateMachine(BaseMonster monster)
    {
        Monster = monster;

        MovementSpeedModifier = 1f;

        MonsterIdleState = new MonsterIdleState(this);
        MonsterChaseState = new MonsterChaseState(this);
        MonsterBaseAttack = new MonsterBaseAttack(this);

        if (monster is ToiletMonster)
        {
            MonsterCenterSkillAttack = new MonsterCenterSkillAttack(this, monster.Stats.GetSkill("SmileMachine_Slam"));
        }
        
        aiEvents = monster.GetComponent<MonsterAIEvents>();
        if (aiEvents == null)
        {
            aiEvents = monster.gameObject.AddComponent<MonsterAIEvents>();
        }
    }
    public void EnableAIEvents()
    {
        if (aiEvents == null) return;

    }

    public void DisableAIEvents()
    {
        if (aiEvents == null) return;
        
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
            if (Monster is ToiletMonster)
            {
                ChangeState(MonsterCenterSkillAttack);
            }
            
        }
    }

    private void HandleRestingPhase()
    {
        ChangeState(MonsterIdleState);
    }
    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
