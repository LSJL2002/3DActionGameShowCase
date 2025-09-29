using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Zenject.SpaceFighter;

public interface IPlayer
{
}

public class PlayerManager : Singleton<PlayerManager>, IPlayer
{
    [field: SerializeField] public PlayerInfo InfoData { get; private set; }

    public PlayerStats Stats { get; private set; }


    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationHash AnimationData { get; private set; }
    //런타임 계산이 필요한 데이터는 이렇게 초기화


    public Animator Animator { get; private set; } //루트모션은 본체에
    public CharacterController Controller { get; private set; }
    public PlayerController Input { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Interaction Interaction { get; private set; }
    public PlayerCombat Combat { get; private set; }


    public PlayerStateMachine stateMachine; //순수 C# 클래스
    public SkillManagers skill;
    public CameraManager camera;
    public DirectionManager direction;
    public VFXManager vFX;
    public HitStopManager hitStop;


    private void Awake()
    {
        //임시함수
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 120;

        AnimationData = new PlayerAnimationHash();
        AnimationData.Initialize();

        Animator ??= GetComponent<Animator>();
        Controller ??= GetComponent<CharacterController>();
        Input ??= GetComponent<PlayerController>();
        ForceReceiver ??= GetComponent<ForceReceiver>();
        Interaction ??= GetComponent<Interaction>();
        Combat ??= GetComponent<PlayerCombat>();

        Stats = new PlayerStats(InfoData.StatData);
        stateMachine = new PlayerStateMachine(this);
        skill ??= GetComponentInChildren<SkillManagers>();
        camera ??= GetComponentInChildren<CameraManager>();
        direction ??= GetComponentInChildren<DirectionManager>();
        vFX ??= GetComponent<VFXManager>();
        hitStop ??= GetComponent<HitStopManager>();

        Stats.OnDie += OnDie;
    }

    void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.LogicUpdate();
        Stats.UpdateSkillBuffer();
    }

    private void FixedUpdate()
    {
        stateMachine.Physicsupdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        gameObject.SetActive(false);
    }

    public void EnableInput(bool active)
    {
        if (active)
        {
            Input.PlayerActions.Enable();
            camera.SetCameraInputEnabled(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Input.PlayerActions.Disable();
            camera.SetCameraInputEnabled(false);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}