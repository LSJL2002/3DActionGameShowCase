using System;
using UnityEngine;

public class MonsterAIEvents : MonoBehaviour
{
    public event Action OnPlayerDetected;
    public event Action OnInAttackRange;
    public event Action RestingPhase;

    private MonsterStateMachine stateMachine;
    private Transform player;
    [SerializeField] private float attackBuffer = 2f;

    [Header("Attack Cooldown")]
    public float attackCooldown = 10f;
    private float lastAttackTime;

    private enum AIMode
    {
        Idle,
        Chase,
        Attack
    }

    private AIMode currentmode = AIMode.Idle;
    private bool processingEnabled = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (!processingEnabled || player == null || stateMachine == null) return;

        stateMachine.Monster.PickPatternByCondition(); //매 프레임마다 선정
        float distance = Vector3.Distance(transform.position, player.position);
        float detectRange = stateMachine.Monster.Stats.DetectRange;
        float attackRange = stateMachine.Monster.GetCurrentSkillRange();

        AIMode newMode = currentmode;

        // Only block attack decisions if already attacking
        if (!stateMachine.isAttacking)
        {
            if (distance <= attackRange - attackBuffer)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    newMode = AIMode.Attack;
                }
                else
                {
                    newMode = AIMode.Idle;
                }
            }
            else if (distance <= detectRange)
            {
                newMode = AIMode.Chase;
            }
            else
            {
                newMode = AIMode.Idle;
            }
        }
        else
        {
            if (distance <= detectRange && distance > attackRange * 0.8f)
                newMode = AIMode.Chase;
        }

        Debug.Log($"{newMode}");
        if (newMode != currentmode)
        {
            currentmode = newMode;

            switch (newMode)
            {
                case AIMode.Attack:
                    OnInAttackRange?.Invoke();
                    lastAttackTime = Time.time;
                    break;
                case AIMode.Chase:
                    stateMachine.Monster.PlayerTarget = player;
                    OnPlayerDetected?.Invoke();
                    break;
                case AIMode.Idle:
                    RestingPhase?.Invoke();
                    break;
            }
        }

        if (currentmode == AIMode.Chase)
        {
            stateMachine.Monster.PlayerTarget = player;
            OnPlayerDetected?.Invoke();
        }
    }


    public void SetStateMachine(MonsterStateMachine sm)
    {
        stateMachine = sm;
    }
    public void Disable()
    {
        processingEnabled = false;
    }

    public void Enable()
    {
        processingEnabled = true;
    }
}
