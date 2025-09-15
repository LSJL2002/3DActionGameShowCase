using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAIEvents : MonoBehaviour
{
    public event Action OnPlayerDetected;
    public event Action<MonsterSkillSO> OnSkillInRange;
    public event Action OnPatternFinished;

    private MonsterStateMachine stateMachine;
    private Transform player;

    [Header("Pattern Settings")]
    public MonsterPatternSO PatternTable;
    private MonsterPattern currentPattern;
    private int currentSkillIndex = 0;

    private bool isInChargeState = false;
    private float chargeEndTime;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null || stateMachine == null) return;

        if (isInChargeState)
        {
            if (Time.time >= chargeEndTime)
            {
                isInChargeState = false;

            }
        }
    }

    public void SetStateMachine(MonsterStateMachine sm)
    {
        stateMachine = sm;
    }

    private void SelectPatternBasedOnCondition()
    {
        if (PatternTable == null) return;

        float hpPercent = (float)stateMachine.Monster.Stats.CurrentHP / stateMachine.Monster.Stats.maxHp;

        MonsterCondition selectedCondition = null;

        foreach (var cond in PatternTable.conditions)
        {
            
        }
    }

    private bool CheckCondition(MonsterCondition cond, float hpPercent)
    {
        if (cond.description.Contains)
    }

}
