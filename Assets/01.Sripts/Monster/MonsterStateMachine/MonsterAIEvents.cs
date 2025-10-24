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
    public float attackCooldown = 10f;
    public float lastAttackTime;

    private enum AIMode { Idle, Chase, Attack }
    private AIMode currentMode = AIMode.Idle;
    private bool processingEnabled = true;

    //FailSafe
    private float idleTimer;
    private const float idleResetTime = 8f;

    private void Awake()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnActiveCharacterChanged += UpdatePlayerReference;
        }

        UpdatePlayerReference(PlayerManager.Instance?.ActiveCharacter);
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnActiveCharacterChanged -= UpdatePlayerReference;
        }
    }

    private void UpdatePlayerReference(PlayerCharacter newPlayer)
    {
        if (newPlayer != null)
        {
            player = newPlayer.transform;
            Debug.Log($"[MonsterAIEvents] Target updated to new player: {newPlayer.name}");
        }
        else
        {
            Debug.LogError("[MonsterAIEvents] Tried to update to null player!");
        }
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
                // 사정거리 들어갈때에는
                if (Time.time >= lastAttackTime + attackCooldown)
                    newMode = AIMode.Attack;
                else
                    newMode = AIMode.Idle; //공격이 불가능하다면 Idle로 변경
            }
            else if (distance <= detectRange - chaseBuffer)
            {
                // 공격 사정거리에 없지만, 플레이어를 인지하는 상태
                newMode = AIMode.Chase;
            }
            else if (distance > detectRange + idleBuffer)
            {
                //너무 멀면 Idle상태
                newMode = AIMode.Idle;
            }
        }
        else
        {
            // 공격을 하면서 사정거리에 벗어나면 다시 따라간다.
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
                    break;
                case AIMode.Chase:
                    stateMachine.Monster.PlayerTarget = player;
                    break;
                case AIMode.Idle:
                    RestingPhase?.Invoke();
                    break;
            }
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

    public void SetStateMachine(MonsterStateMachine sm)
    {
        stateMachine = sm;
    }

    public void Disable() => processingEnabled = false;
    public void Enable() => processingEnabled = true;
}
