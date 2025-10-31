using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine.TextCore.Text;

public class BaseMonster : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] public MonsterAnimationData animationData;
    public MonsterPatternSO patternConfig;
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public MonsterStatHandler Stats { get; private set; }
    public MonsterAIEvents aiEvents { get; private set; }
    public Transform PlayerTarget { get; set; }
    public GameObject AreaEffectPoint;
    public bool IsDead { get; set; }
    [HideInInspector] public bool hasStartedCombat = false;
    private readonly List<GameObject> activeAOEs = new List<GameObject>();
    [HideInInspector] public bool hasDetectedPlayer = false;
    public MonsterStateMachine stateMachine;

    protected MonsterPatternSO.PatternEntry currentPattern;
    protected int currentPatternPriority = -1; //아직 선택된 패턴이 없음
    protected int currentStepIndex = 0;
    protected bool isRunningPattern = false;
    private bool ignoreDistanceCheck = false;
    public Collider baseAttackCollider;
    [Header("Movement & Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 verticalVelocity;
    private bool isGrounded;

    protected virtual void Awake()
    {
        animationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<MonsterStatHandler>();
        Controller = GetComponent<CharacterController>();

        aiEvents = GetComponent<MonsterAIEvents>() ?? gameObject.AddComponent<MonsterAIEvents>();
        aiEvents.SetStateMachine(stateMachine);
    }

    protected virtual void Start()
    {
        stateMachine = new MonsterStateMachine(this);
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
        PlayerTarget = GameObject.FindWithTag("Player").transform;

        PlayerManager.Instance.OnActiveCharacterChanged += OnActiveCharacterChanged;
        ResetPatternConditions();

        if (Agent != null) Agent.speed = stateMachine.MovementSpeed;
    }

    protected virtual void Update()
    {
        stateMachine.HandleInput();
        stateMachine.LogicUpdate();
        if (!isRunningPattern && !IsDead && stateMachine.CurrentState is MonsterIdleState)
        {
            PickPatternByCondition();
        }
        ApplyGravity();
    }
    private void OnActiveCharacterChanged(PlayerCharacter newCharacter)
    {
        if (newCharacter != null)
            PlayerTarget = newCharacter.transform;
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnActiveCharacterChanged -= OnActiveCharacterChanged;
    }
    private void ApplyGravity()
    {
        if (Controller == null || IsDead) return;

        // Simple ground check using a raycast or sphere
        isGrounded = Physics.CheckSphere(transform.position + Vector3.up * 0.1f, groundCheckDistance, groundMask);

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        // Apply gravity acceleration
        verticalVelocity.y += gravity * Time.deltaTime;

        // Move the controller vertically
        Controller.Move(verticalVelocity * Time.deltaTime);
    }

    protected virtual void FixedUpdate() => stateMachine.Physicsupdate();
    protected virtual void OnEnable() => stateMachine?.EnableAIEvents();
    protected virtual void OnDisable() => stateMachine?.DisableAIEvents();


    //패턴 로직
    public bool IsPatternRunning() => isRunningPattern;

    public void PickPatternByCondition()
    {
        if (patternConfig == null || isRunningPattern) return;

        float hpPercent = (Stats.CurrentHP / Stats.maxHp) * 100f;
        float distance = PlayerTarget != null ? Vector3.Distance(transform.position, PlayerTarget.position) : Mathf.Infinity;

        var validConditions = patternConfig.GetValidConditions(hpPercent, distance, hasStartedCombat);
        if (validConditions == null || validConditions.Count == 0) return;

        var chosenCondition = validConditions[0];
        if (chosenCondition.possiblePatternIds.Count == 0) return;

        int patternId = chosenCondition.possiblePatternIds[UnityEngine.Random.Range(0, chosenCondition.possiblePatternIds.Count)];
        currentPattern = patternConfig.GetPatternById(patternId);
        if (currentPattern == null) return;

        stateMachine.RangeMultiplier = chosenCondition.rangeMultiplier;
        stateMachine.PreCastTimeMultiplier = chosenCondition.preCastTimeMultiplier;
        stateMachine.EffectValueMultiplier = chosenCondition.effectValueMultiplier;
        ignoreDistanceCheck = chosenCondition.ignoreDistanceCheck;

        currentPatternPriority = chosenCondition.priority;

        currentStepIndex = 0;
        StartCoroutine(RunPattern());
    }

    private IEnumerator RunPattern()
    {
        isRunningPattern = true;

        while (currentPattern != null && currentStepIndex < currentPattern.states.Count)
        {
            if (IsDead) yield break;

            var stateEnum = currentPattern.states[currentStepIndex];
            var attackState = GetStateFromEnum(stateEnum);
            if (attackState == null)
            {
                currentStepIndex++;
                continue;
            }

            float skillRange = GetSkillRangeFromState(attackState);

            // --- Wait until player is in range (unless ignoring distance) ---
            if (!ignoreDistanceCheck)
            {
                float waitTime = 0f;
                float maxWait = 5f; // maximum wait time
                while (!IsDead && PlayerTarget != null && waitTime < maxWait)
                {
                    if (Vector3.Distance(transform.position, PlayerTarget.position) <= skillRange)
                        break; // player in range → attack

                    waitTime += Time.deltaTime;
                    yield return null;
                }
                // after maxWait, attack anyway
            }

            // --- Perform attack ---
            stateMachine.isAttacking = true;
            stateMachine.ChangeState(attackState);

            if (!hasStartedCombat)
                hasStartedCombat = true;

            // Wait until attack finishes
            yield return new WaitUntil(() => !stateMachine.isAttacking);

            // --- Return to Idle for a moment before next attack ---
            yield return new WaitForSeconds(0.3f); // short reset time (tweak as needed)
            stateMachine.ChangeState(stateMachine.MonsterIdleState);

            currentStepIndex++;
        }

        // --- Pattern finished, cooldown before new one ---
        float cooldown = UnityEngine.Random.Range(1f, 3f);
        yield return new WaitForSeconds(cooldown);
        
        currentPattern = null;
        currentPatternPriority = -1;
        isRunningPattern = false;

        // Return to idle after finishing pattern
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    public float GetCurrentSkillRange()
    {
        if (currentPattern != null && currentStepIndex < currentPattern.states.Count)
        {
            var stateEnum = currentPattern.states[currentStepIndex];
            var state = GetStateFromEnum(stateEnum);
            if (state != null)
            {
                return GetSkillRangeFromState(state);
            }
        }
        //쿨 다운인 상태에는 몬스터 기본 공격 사거리를 사용
        return Stats.AttackRange;
    }


    //Virtual, 각 몬스터에게 스킬을 연결할떄
    protected virtual MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        return null;
    }

    protected virtual float GetSkillRangeFromState(MonsterBaseState state)
    {
        return Stats.AttackRange; // 없다면, 몬스터의 기본 사거리 사용
    }

    public void OnDeathAnimationComplete()
    {
        BattleManager.Instance.HandleMonsterDie();
    }

    public void OnAttackAnimationComplete()
    {
        if (stateMachine.CurrentState is MonsterBaseState)
        {
            (stateMachine.CurrentState as MonsterBaseState).OnAnimationComplete();
        }
    }

    public void OnAttackHitEvent()
    {
        (stateMachine.CurrentState as MonsterBaseState)?.OnAttackHit();
    }

    public virtual void OnTakeDamage(int amount)
    {
        float damage = Mathf.Max(1, amount - Stats.Defense);
        Stats.CurrentHP -= damage;
        Stats.UpdateHealthUI();
        if (Stats.CurrentHP <= 0 && !IsDead)
        {
            Stats.Die();
            IsDead = true;
            Stats.CurrentHP = 0;
            stateMachine.ChangeState(stateMachine.MonsterDeathState);
        }
    }

    public void ApplyEffect(MonsterEffectType effectType, Vector3 sourcePosition, float effectValue = 0f, float duration = 0f)
    {
        // 플레이어게만 적용
    }

    public void RegisterAOE(GameObject aoe)
    {
        if (aoe != null && !activeAOEs.Contains(aoe)) activeAOEs.Add(aoe);
    }

    public void UnregisterAOE(GameObject aoe)
    {
        if (aoe != null && activeAOEs.Contains(aoe)) activeAOEs.Remove(aoe);
    }

    public void ClearAOEs()
    {
        foreach (var aoe in activeAOEs)
            if (aoe != null) Destroy(aoe);
        activeAOEs.Clear();
    }

    protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsDead) return;

        (stateMachine.CurrentState as MonsterBaseState)?.OnControllerColliderHit(hit);
    }
    private void ResetPatternConditions()
    {
        if (patternConfig == null) return;

        foreach (var cond in patternConfig.conditions)
        {
            cond.hasTriggered = false;
        }
    }
}
