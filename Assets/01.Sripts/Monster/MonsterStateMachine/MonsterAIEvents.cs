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

    [Header("Attack Cooldown")]
    public float attackCooldown = 10f; // seconds between attacks
    private float lastAttackTime;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        float detectRange = stateMachine.Monster.Stats.DetectRange;
        float attackRange = stateMachine.Monster.Stats.AttackRange;

        Debug.Log("Aievent");

        // ---- 우선순위: 공격 범위 먼저 ----
        if (distance <= attackRange && !stateMachine.isAttacking)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                OnInAttackRange?.Invoke();
                Debug.Log("Attack");
                lastAttackTime = Time.time;
            }
            else
            {
                RestingPhase?.Invoke();
                Debug.Log("Idle");
            }
        }
        // ---- 공격 범위 밖, 탐지 범위 안 ----
        else if (distance <= detectRange && !stateMachine.isAttacking)
        {
            stateMachine.Monster.PlayerTarget = player;
            OnPlayerDetected?.Invoke();
            Debug.Log("Detected");
        }
    }

    public void SetStateMachine(MonsterStateMachine sm)
    {
        stateMachine = sm;
    }

}
