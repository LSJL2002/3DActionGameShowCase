using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Zenject.SpaceFighter;

public interface IPlayer
{
    public void LockOnInput(int val);
}

public class PlayerManager : Singleton<PlayerManager>, IPlayer
{
    [field: SerializeField] public PlayerSO Data { get; private set; }
    public PlayerStats Stats { get; private set; }


    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationHash AnimationData { get; private set; }
    //런타임 계산이 필요한 데이터는 이렇게 초기화

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public PlayerController Input { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Interaction Interaction { get; private set; }

    private PlayerStateMachine stateMachine; //순수 C# 클래스

    private void Awake()
    {
        //임시함수
        Cursor.lockState = CursorLockMode.Locked;


        AnimationData.Initialize();
        Animator ??= GetComponentInChildren<Animator>();
        Controller ??= GetComponent<CharacterController>();
        Input ??= GetComponent<PlayerController>();
        ForceReceiver ??= GetComponent<ForceReceiver>();
        Interaction ??= GetComponent<Interaction>();
        Stats = new PlayerStats(Data);

        stateMachine = new PlayerStateMachine(this);

        Stats.OnDie += OnDie;
    }

    void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.Physicsupdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

    // 외부에서 이 메서드로만 접근
    public void LockOnInput(int val)
    {
        //Controller.LockOnInput(val);
    }
}