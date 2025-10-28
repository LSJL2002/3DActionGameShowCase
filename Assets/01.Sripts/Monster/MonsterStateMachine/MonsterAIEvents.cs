using System;
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
    public float attackCooldown = 3f;
    public float lastAttackTime;

    private enum AIMode { Idle, Chase, CombatIdle, Attack }
    private AIMode currentMode = AIMode.Idle;
    private bool processingEnabled = true;
    private bool combatIdleStarted = false;

    //FailSafe
    private float idleTimer;
    private const float idleResetTime = 8f;

    private void Awake()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnActiveCharacterChanged += UpdatePlayerReference;

        UpdatePlayerReference(PlayerManager.Instance?.ActiveCharacter);
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnActiveCharacterChanged -= UpdatePlayerReference;
    }

    private void UpdatePlayerReference(PlayerCharacter newPlayer)
    {
        if (newPlayer != null)
            player = newPlayer.transform;
    }

    private void Update()
    {
        if (!processingEnabled || player == null || stateMachine == null)
            return;

        if (PlayerManager.Instance.ActiveCharacter.Ability.IsDeath)
        {
            if (currentMode != AIMode.Idle)
            {
                currentMode = AIMode.Idle;
                RestingPhase?.Invoke();
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
            }
            return;
        }

        stateMachine.Monster.PickPatternByCondition();
        float distance = Vector3.Distance(transform.position, player.position);
        float detectRange = stateMachine.Monster.Stats.DetectRange;
        float attackRange = stateMachine.Monster.GetCurrentSkillRange();

        AIMode newMode = currentMode;

        if (!stateMachine.isAttacking)
        {
            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    newMode = AIMode.Attack; // ready to attack
                }
                else
                {
                    newMode = AIMode.CombatIdle; // cooldown not done → wait but check continuously
                }
            }
            else if (distance <= detectRange - chaseBuffer)
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
            if (distance > attackRange * 1.2f)
                newMode = AIMode.Chase;
        }

        // Switch state if changed
        if (newMode != currentMode)
        {
            currentMode = newMode;
            switch (newMode)
            {
                case AIMode.Attack:
                    OnInAttackRange?.Invoke();
                    lastAttackTime = Time.time;
                    combatIdleStarted = false;
                    break;
                case AIMode.Chase:
                    stateMachine.Monster.PlayerTarget = player;
                    combatIdleStarted = false;
                    break;
                case AIMode.CombatIdle:
                    stateMachine.Monster.PlayerTarget = player;
                    if (!combatIdleStarted)
                    {
                        stateMachine.ChangeState(stateMachine.MonsterIdleState);
                        combatIdleStarted = true;
                    }
                    break;
                case AIMode.Idle:
                    RestingPhase?.Invoke();
                    combatIdleStarted = false;
                    break;
            }
        }

        // Continuously check for cooldown even while waiting
        if (currentMode == AIMode.CombatIdle && Time.time >= lastAttackTime + attackCooldown)
        {
            currentMode = AIMode.Attack;
            OnInAttackRange?.Invoke();
            lastAttackTime = Time.time;
        }

        if (currentMode == AIMode.Chase)
        {
            stateMachine.Monster.PlayerTarget = player;
            OnPlayerDetected?.Invoke();
        }

        if (currentMode == AIMode.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleResetTime)
            {
                idleTimer = 0;
                currentMode = AIMode.Chase;
                OnPlayerDetected?.Invoke();
            }
        }
    }



    public void SetStateMachine(MonsterStateMachine sm) => stateMachine = sm;
    public void Disable() => processingEnabled = false;
    public void Enable() => processingEnabled = true;
}
