using System;
using UniRx.Triggers;
using UnityEngine;

public class MonsterAIEvents : MonoBehaviour
{
    public event Action OnPlayerDetected;
    public event Action OnInAttackRange;
    public event Action RestingPhase;

    private MonsterStateMachine stateMachine;
    private Transform player;
    [SerializeField] private float chaseBuffer = 0.5f;
    [SerializeField] private float idleBuffer = 0.5f;
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
    private PlayerManager PlayerManager;

    private void Awake()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            PlayerManager = playerObj.GetComponent<PlayerManager>();

            if (PlayerManager == null)
                Debug.LogError("PlayerManager component not found on Player object!");
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }
    }

    private void Update()
    {
        if (!processingEnabled || player == null || stateMachine == null) return;

        if (PlayerManager.Instance.ActiveCharacter.Stats.IsDead)
        {
            if (currentmode != AIMode.Idle)
            {
                currentmode = AIMode.Idle;
                RestingPhase?.Invoke();
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
            }
            return;
        }
        stateMachine.Monster.PickPatternByCondition();
        float distance = Vector3.Distance(transform.position, player.position);
        float detectRange = stateMachine.Monster.Stats.DetectRange;
        float attackRange = stateMachine.Monster.GetCurrentSkillRange();

        AIMode newMode = currentmode;

        if (!stateMachine.isAttacking)
        {
            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                    newMode = AIMode.Attack;
                else
                    newMode = AIMode.Idle;
            }
            else if (distance <= detectRange - chaseBuffer)
            {
                newMode = AIMode.Chase;
            }
            else if (distance > detectRange + idleBuffer)
            {
                newMode = AIMode.Idle;
            }
        }
        else
        {
            if (distance <= detectRange && distance > attackRange * 1.2f)
                newMode = AIMode.Chase;
        }

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
