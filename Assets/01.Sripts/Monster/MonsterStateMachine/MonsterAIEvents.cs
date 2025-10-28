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

    private enum AIMode { Chase, CombatIdle, Attack }
    private AIMode currentMode = AIMode.CombatIdle;
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
            currentMode = AIMode.CombatIdle;
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
                newMode = Time.time >= lastAttackTime + attackCooldown ? AIMode.Attack : AIMode.CombatIdle;
            }
            else if (distance <= detectRange - chaseBuffer)
                newMode = AIMode.Chase;
            else
                newMode = AIMode.CombatIdle; // keep idle animation
        }
        else
        {
            if (distance > attackRange * 1.2f)
                newMode = AIMode.Chase;
        }

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
                        combatIdleStarted = true;
                        RestingPhase?.Invoke();
                    }
                    break;
            }
        }

        // Continuously check for cooldown
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
    }


    public void SetStateMachine(MonsterStateMachine sm) => stateMachine = sm;
    public void Disable() => processingEnabled = false;
    public void Enable() => processingEnabled = true;
}
