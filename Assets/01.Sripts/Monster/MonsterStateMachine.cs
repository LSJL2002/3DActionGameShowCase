using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }

    public GameObject Target { get; private set; }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
