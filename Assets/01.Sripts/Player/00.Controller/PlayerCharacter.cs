using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; } = CharacterType.Yuki;
    [field: SerializeField] public PlayerInfo InfoData { get; private set; }

    public PlayerAttribute Attr { get; private set; }
    public EventHub Hub { get; private set; }
    public AbilitySystem Ability { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerAnimationHash AnimationData { get; private set; }
    public InputSystem Input { get; private set; }
    //런타임 계산이 필요한 데이터는 이렇게 초기화

    // ================= Component ====================
    public Animator Animator { get; private set; } //루트모션은 본체에
    public CharacterController Controller { get; private set; }
    public PlayerAttackController Attack { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Interaction Interaction { get; private set; }
    public HitboxOverlap Hit { get; private set; }
    public PlayerDamageable Damageable { get; private set; }
    public ActionHandler Action { get; private set; }
    public PlayerVitals Vital { get; private set; }

    public Transform Face;
    public Transform Body;

    // ================ Managers ================
    public PlayerManager PlayerManager { get; private set; }
    public CameraManager _camera { get; private set; }
    public SkillManagers skill { get; private set; }
    public EventManager _event { get; private set; }
    public VFXManager vFX { get; private set; }
    public HitStopManager hitStop { get; private set; }
    public DirectionManager direction { get; private set; }

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

    private void Awake()
    {
        AnimationData = new PlayerAnimationHash();
        AnimationData.Initialize();

        Animator ??= GetComponent<Animator>();
        Controller ??= GetComponent<CharacterController>();
        Attack ??= GetComponent<PlayerAttackController>();
        ForceReceiver ??= GetComponent<ForceReceiver>();
        Interaction ??= GetComponent<Interaction>();
        Damageable ??= GetComponent<PlayerDamageable>();
        Hit ??= GetComponent<HitboxOverlap>();
        Action ??= GetComponent<ActionHandler>();
        Vital ??= GetComponent<PlayerVitals>();

        Input = new InputSystem();
        Hub = new EventHub();
        Attr = new PlayerAttribute(InfoData, Hub);
        Attr.Resource.OnDie += OnDie;

        StateMachine = new PlayerStateMachine(this);
        Ability = gameObject.AddComponent<AbilitySystem>();
        Ability.Initialize(Attr, StateMachine, Input);

        Input.OnZoom += OnZoomInput;
        Input.OnMenuToggle += OnMenuToggle;
        Input.OnInventoryToggle += OnInventoryToggle;
        Input.OnCameraLockOn += OnLockOnToggle;
    }
    private void OnDestroy()
    {
        Input.OnZoom -= OnZoomInput;
        Input.OnMenuToggle -= OnMenuToggle;
        Input.OnInventoryToggle -= OnInventoryToggle;
        Input.OnCameraLockOn -= OnLockOnToggle;
    }

    private void Update()
    {
        StateMachine.HandleInput(); // FSM 업데이트
        StateMachine.LogicUpdate();  

        Attr.Update(); // Attribute 계산 업데이트
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
        if (active) Input.Enable(); 
        else Input.Disable();
    }

    private void OnZoomInput(float delta)
    {
        var vcam = _camera.FreeLookCam;
        if (vcam == null) return;

        float zoomSpeed = 3f;
        float fov = vcam.Lens.FieldOfView - Mathf.Sign(delta) * zoomSpeed;
        vcam.Lens.FieldOfView = Mathf.Clamp(fov, 10f, 70f);
    }

    private bool isPaused = false;
    private void OnMenuToggle()
    {
        isPaused = !isPaused;
        GameManager.Instance.PauseGame(isPaused);
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        _camera.Volume_Blur.enabled = isPaused;
    }
    private void OnInventoryToggle()
    {
        if (direction.inventory == null) return;
        var inven = direction.inventory;
        bool isActive = !inven.activeSelf;
        inven.SetActive(isActive);

        // 인벤토리 켜면 시간 멈추기, 끄면 다시 정상
        Time.timeScale = isActive ? 0f : 1f;

        // 옵션: 커서 락 해제
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
    }
    private void OnLockOnToggle()
    {
        var cam = _camera;
        cam.ToggleLockOnTarget(null); // 무조건 락온 해제
    }
}