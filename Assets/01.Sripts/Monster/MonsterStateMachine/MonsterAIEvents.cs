using System;
using UnityEngine;
public enum AIMode
{
    CombatIdle,
    Chase,
    Attack
}
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
    [SerializeField] private AIMode currentMode = AIMode.CombatIdle; // 인스펙터에서 수정 가능하도록 설정
    private bool processingEnabled = true;
    private bool combatIdleStarted = false;
    public bool hasChosenPattern = false;

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

        if (!hasChosenPattern)
        {
            stateMachine.Monster.PickPatternByCondition();
            hasChosenPattern = true;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        float detectRange = stateMachine.Monster.Stats.DetectRange;
        float attackRange = stateMachine.Monster.GetCurrentSkillRange();

        AIMode newMode = currentMode;

        if (!stateMachine.isAttacking)
        {
            if (distance < detectRange - chaseBuffer)
            {
                currentMode = AIMode.Chase;
            }

            if (distance <= attackRange)
            {
                currentMode = Time.time >= lastAttackTime + attackCooldown ? AIMode.Attack : AIMode.CombatIdle;
            }
            else if (distance <= detectRange - chaseBuffer)
            {
                currentMode = AIMode.Chase;
            }
            else
            {
                currentMode = AIMode.CombatIdle; // keep idle animation
            }
        }
        else
        {/*
            if (distance > attackRange * 1.2f)
            {
                newMode = AIMode.Chase;
            }*/
        }


        switch (currentMode)
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


        // Continuously check for cooldown
        if (currentMode == AIMode.CombatIdle && Time.time >= lastAttackTime + attackCooldown && distance <= attackRange)
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

        if (stateMachine.isAttacking && Time.time >= lastAttackTime + attackCooldown + 1f)
        {
            stateMachine.isAttacking = false;
            currentMode = AIMode.CombatIdle;
        }
    }

    public void SetStateMachine(MonsterStateMachine sm) => stateMachine = sm;
    public void Disable() => processingEnabled = false;
    public void Enable() => processingEnabled = true;
}