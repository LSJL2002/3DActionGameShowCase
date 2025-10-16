using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; } = CharacterType.Yuki;
    [field: SerializeField] public PlayerInfo InfoData { get; private set; }

    public PlayerStats Stats { get; private set; }
    public PlayerAnimationHash AnimationData { get; private set; }
    //런타임 계산이 필요한 데이터는 이렇게 초기화
    public PlayerStateMachine StateMachine { get; private set; }
    public Animator Animator { get; private set; } //루트모션은 본체에
    public CharacterController Controller { get; private set; }
    public PlayerController Input { get; private set; }
    public PlayerAttackController Attack { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Interaction Interaction { get; private set; }
    public HitboxOverlap Hit { get; private set; }
    public PlayerDamageable Damageable { get; private set; }
    public ActionHandler Action { get; private set; }
    public PlayerVitals Vital { get; private set; }

    public Transform Face;
    public Transform Body;

    // ============= Managers =============
    public PlayerManager PlayerManager { get; private set; }
    public CameraManager _camera { get; private set; }
    public SkillManagers skill { get; private set; }
    public EventManager _event { get; private set; }
    public VFXManager vFX { get; private set; }
    public HitStopManager hitStop { get; private set; }
    public DirectionManager direction { get; private set; }

    private void Awake()
    {
        AnimationData = new PlayerAnimationHash();
        AnimationData.Initialize();

        Animator ??= GetComponent<Animator>();
        Controller ??= GetComponent<CharacterController>();
        Input ??= GetComponent<PlayerController>();
        Attack ??= GetComponent<PlayerAttackController>();
        ForceReceiver ??= GetComponent<ForceReceiver>();
        Interaction ??= GetComponent<Interaction>();
        Damageable ??= GetComponent<PlayerDamageable>();
        Hit ??= GetComponent<HitboxOverlap>();
        Action ??= GetComponent<ActionHandler>();
        Vital ??= GetComponent<PlayerVitals>();

        Stats = new PlayerStats(InfoData.StatData);
        StateMachine = new PlayerStateMachine(this);

        Stats.OnDie += OnDie;
    }

    public void InitManagers(
        PlayerManager playerManager,
        CameraManager cam,
        SkillManagers skl,
        EventManager evt,
        VFXManager vfx,
        HitStopManager hit,
        DirectionManager dir
    )
    {
        PlayerManager = playerManager;
        _camera = cam;
        skill = skl;
        _event = evt;
        vFX = vfx;
        hitStop = hit;
        direction = dir;
    }

    private void Update()
    {
        StateMachine.HandleInput();    // 플레이어 입력 처리
        StateMachine.LogicUpdate();    // FSM 상태별 논리 업데이트
        StateMachine.HandleUpdate();   // 현재 BattleModule 업데이트
        StateMachine.HandleSkillUpdate();
        Stats.Update();                // 스탯 처리    
    }

    private void FixedUpdate()
    {
        StateMachine.Physicsupdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        EnableCharacterInput(false);
        gameObject.SetActive(false);
    }

    public void EnableCharacterInput(bool active)
    {
        if (active) Input.PlayerActions.Enable();
        else Input.PlayerActions.Disable();
    }
}