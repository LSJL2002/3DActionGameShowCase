using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAIEvents : MonoBehaviour
{
    public event Action OnPlayerDetected;
    public event Action OnPlayerLost;
    public event Action OnInAttackRange;

    private MonsterStateMachine stateMachine;
    private Transform player;

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

        // 플레이어가 감지 범위 안에 있을시
        if (distance <= detectRange && !stateMachine.isAttacking)
        {
            stateMachine.Monster.PlayerTarget = player;
            OnPlayerDetected?.Invoke();
            Debug.Log("Detected");
        }

        // 플레이어가 감지 범위 나갈시
        else if (distance > detectRange && !stateMachine.isAttacking)
        {
            stateMachine.Monster.PlayerTarget = null;
            OnPlayerLost?.Invoke();
            Debug.Log("Lost");
        }

        // 공격 사거리 입장시
        if (distance <= attackRange && !stateMachine.isAttacking)
        {
            OnInAttackRange?.Invoke();
            Debug.Log("Attack");
        }
    }

    public void SetStateMachine(MonsterStateMachine sm)
    {
        stateMachine = sm;
    }

}
