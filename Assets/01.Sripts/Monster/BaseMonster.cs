using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using System;

public class BaseMonster : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] public MonsterAnimationData animationData;

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public MonsterStatHandler Stats { get; private set; }
    public MonsterAIEvents aiEvents { get; private set; }
    public Transform PlayerTarget { get; set; }

    public event Action OnAttackAnimationCompleteEvent;


    public MonsterStateMachine stateMachine;

    protected virtual void Awake()
    {
        animationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<MonsterStatHandler>();

        stateMachine = new MonsterStateMachine(this);

        aiEvents = GetComponent<MonsterAIEvents>();
        if (aiEvents == null)
        {
            aiEvents = gameObject.AddComponent<MonsterAIEvents>();
        }

        aiEvents.SetStateMachine(stateMachine);
    }

    protected virtual void Start()
    {
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
        PlayerTarget = GameObject.FindWithTag("Player").transform;
        if (Agent != null)
        {
            Agent.speed = stateMachine.MovementSpeed;
        }
    }

    protected virtual void Update()
    {
        stateMachine.HandleInput();
        stateMachine.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.Physicsupdate();
    }

    protected virtual void OnEnable()
    {
        stateMachine?.EnableAIEvents();
    }

    protected virtual void OnDisable()
    {
        stateMachine?.DisableAIEvents();
    }

    public void OnAttackAnimationComplete() //나중에 삭제
    {
        Debug.Log("Finished");
        stateMachine.isAttacking = false;
    }

    public void OnAttackHitEvent()
    {
        Debug.Log("OnHit");
        (stateMachine.CurrentState as MonsterBaseState)?.OnAttackHit();
    }

    public virtual void OnTakeDamage(int amount)
    {
        Stats.CurrentHP -= amount - Stats.Defense;
        // 체력 변동 애니메이션 추가
        if (Stats.CurrentHP <= 0)
        {
            Stats.Die();
        }
    }
    

    //메소드 추가 몬스터 애니메이션
}
