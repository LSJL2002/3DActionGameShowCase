using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAIEvents : MonoBehaviour
{
    public event Action OnPlayerDetected;
    public event Action OnInAttackRange;
    public event Action RestingPhase;

    private MonsterStateMachine stateMachine;
    private Transform player;
    [SerializeField] private float attackBuffer = 1f;

    [Header("Attack Cooldown")]
    public float attackCooldown = 10f;
    private float lastAttackTime;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null || stateMachine == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        float detectRange = stateMachine.Monster.Stats.DetectRange;
        float attackRange = stateMachine.Monster.Stats.AttackRange;

        if (!stateMachine.isAttacking)
        {
            if (distance <= attackRange - attackBuffer)
            {
                // Safely within attack range
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    OnInAttackRange?.Invoke();
                    lastAttackTime = Time.time;
                }
                else
                {
                    RestingPhase?.Invoke(); // cooldown
                }
            }
            else if (distance <= detectRange)
            {
                // Still need to move closer → chase
                stateMachine.Monster.PlayerTarget = player;
                OnPlayerDetected?.Invoke();
            }
            else
            {
                // Out of detect range → idle
                RestingPhase?.Invoke();
            }
        }
    }

    public void SetStateMachine(MonsterStateMachine sm)
    {
        stateMachine = sm;
    }
}
