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
    public event Action OnAttackAnimationCompleteEvent;
    private readonly List<GameObject> activeAOEs = new List<GameObject>();

    public MonsterStateMachine stateMachine;

    protected MonsterPatternSO.PatternEntry currentPattern;
    protected int currentPatternPriority = -1; //아직 선택된 패턴이 없음
    protected int currentStepIndex = 0;
    protected bool isRunningPattern = false;
    private bool ignoreDistanceCheck = false;

    // 체력이 변경될 때 호출될 이벤트
    public static event System.Action OnEnemyHealthChanged;

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

        if (Agent != null) Agent.speed = stateMachine.MovementSpeed;
    }

    protected virtual void Update()
    {
        stateMachine.HandleInput();
        stateMachine.LogicUpdate();
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

        if (isRunningPattern && chosenCondition.priority <= currentPatternPriority)
        {
            return;
        }

        if (isRunningPattern)
        {
            StopAllCoroutines();
            isRunningPattern = false;
            currentPattern = null;
            currentStepIndex = 0;
        }

        int patternId = chosenCondition.possiblePatternIds[UnityEngine.Random.Range(0, chosenCondition.possiblePatternIds.Count)];
        currentPattern = patternConfig.GetPatternById(patternId);
        if (currentPattern == null) return;
        stateMachine.RangeMultiplier = chosenCondition.rangeMultiplier;
        stateMachine.PreCastTimeMultiplier = chosenCondition.preCastTimeMultiplier;
        stateMachine.EffectValueMultiplier = chosenCondition.effectValueMultiplier;
        ignoreDistanceCheck = chosenCondition.ignoreDistanceCheck;
        //Debug.Log($"{name} - Picked conditionId={chosenCondition.id} (priority={chosenCondition.priority}) → patternId={patternId}");

        currentStepIndex = 0;
        StartCoroutine(RunPattern());
    }

    public void ForceRunPattern(MonsterPatternSO.PatternEntry pattern, MonsterPatternSO.PatternCondition conditions)
    {
        //미래용
        if (pattern == null)
        {
            return;
        }

        StopAllCoroutines();
        currentPattern = pattern;
        currentStepIndex = 0;
        isRunningPattern = false;

        stateMachine.RangeMultiplier = conditions.rangeMultiplier;
        stateMachine.PreCastTimeMultiplier = conditions.preCastTimeMultiplier;
        stateMachine.EffectValueMultiplier = conditions.effectValueMultiplier;

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
            if (!ignoreDistanceCheck) // If ignore distance check is true, then just perform the attack
            {
                float waitTime = 0f;
                bool inRange = false;
                yield return new WaitUntil(() =>
                {
                    waitTime += Time.deltaTime;
                    inRange = (PlayerTarget != null && Vector3.Distance(transform.position, PlayerTarget.position) <= skillRange);
                    return inRange || waitTime >= 5f;
                });

                if (!inRange)
                {
                    Debug.Log($"{name} stopped pattern {currentPattern.id} after 5s timeout");
                    break;
                }
            }
            // --- Perform attack ---
            stateMachine.isAttacking = true;
            stateMachine.ChangeState(attackState);
            if (!hasStartedCombat)
            {
                hasStartedCombat = true;
            }

            // Wait until attack finishes
            yield return new WaitUntil(() => !stateMachine.isAttacking);

            // --- Return to Idle after attack ---
            if (!(stateMachine.CurrentState is MonsterIdleState))
            {
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForSeconds(0.3f); // lock in Idle for a bit
            }

            currentStepIndex++;
            yield return new WaitForSeconds(0.2f); // small delay between steps
        }
        float cooldown = UnityEngine.Random.Range(1f, 3f);
        yield return new WaitForSeconds(cooldown);
        currentPattern = null;
        currentPatternPriority = -1;
        isRunningPattern = false;
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
        Debug.LogWarning($"{name} - BaseMonster.GetStateFromEnum not overridden!");
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
        stateMachine.isAttacking = false;
        OnAttackAnimationCompleteEvent?.Invoke();
    }

    public void OnAttackHitEvent()
    {
        (stateMachine.CurrentState as MonsterBaseState)?.OnAttackHit();
    }

    public virtual void OnTakeDamage(int amount)
    {
        float damage = Mathf.Max(1, amount - Stats.Defense);
        Stats.CurrentHP -= damage;
        Stats.ApplyDamage(amount);

        if (Stats.CurrentHP <= 0 && !IsDead)
        {
            Stats.Die();
            IsDead = true;
            Stats.CurrentHP = 0;
            stateMachine.ChangeState(stateMachine.MonsterDeathState);
        }

        OnEnemyHealthChanged?.Invoke(); // 체력이 변경될 때 이벤트 호출
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
}
