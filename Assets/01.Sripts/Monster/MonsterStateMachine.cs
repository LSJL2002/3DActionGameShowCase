using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }

    public GameObject Target { get; private set; }
    public MonsterIdleState MonsterIdleState { get; }

    public MonsterStateMachine(BaseMonster monster)
    {
        Monster = monster;

        MonsterIdleState = new MonsterIdleState(this);
    }
    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
