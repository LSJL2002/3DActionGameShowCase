using System.Collections;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    // 오브젝트에 부착할 스크립트
    [Header("Refs")]
    public Transform targetObject;
    public Transform lookObject;
    public Rigidbody rb;
    public Animator anim;
    public GameObject chatUI;

    [Header("Params")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 360f;
    public float moveSpeedThreshold = 0.1f;  // 이동 판정 기준

    [Header("VFX")]
    public GameObject moveFx;

    [HideInInspector] public bool isAttack;
    [HideInInspector] public bool isTalkMode;
    [HideInInspector] public Vector3 cachedAnchorLocalPos; // 캐릭터 중심으로 처음에 고정한 오브젝트 위치
    [HideInInspector] public CursorLockMode cachedLockMode;  // 커서
    [HideInInspector] public bool cachedCursorVisible; // 커서가 보이고 안보이고하는 bool값

    public CompanionUI ui;   // UIManager로 띄우는 컴패니언 UI

    public CompanionStateMachine Sm { get; private set; }

    void Awake()
    {
        if (targetObject) cachedAnchorLocalPos = targetObject.localPosition;
        Sm = new CompanionStateMachine(this);
        Sm.ChangeState(new CompanionIdleState(Sm)); // 시작 상태: Follow (원하면 IdleState로 변경)
    }

    private void OnEnable()
    {
        // 씬 재입장 대비: 참조 리바인드 + 매니저 준비 기다리기
        StartCoroutine(InitRoutine());

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnActiveCharacterChanged += RebindTargets;
        }

        // 🔹 구독은 OnEnable에서만
        if (EventsManager.Instance != null)
        {
            EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleStart, BattleStart);
            EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleClear, BattleClear);
        }
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnActiveCharacterChanged -= RebindTargets;

        // 🔹 구독 해제는 OnDisable에서
        if (EventsManager.Instance != null)
        {
            EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleStart, BattleStart);
            EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleClear, BattleClear);
        }
    }

    private IEnumerator InitRoutine()
    {
        // PlayerManager, ActiveCharacter 준비될 때까지 대기
        yield return new WaitUntil(() => PlayerManager.Instance != null && PlayerManager.Instance.ActiveCharacter != null);
        RebindTargets(PlayerManager.Instance.ActiveCharacter);
    }

    private void RebindTargets(PlayerCharacter newChar)
    {
        if (newChar == null) return;

        // 활성 캐릭터 하위의 FollowObject 탐색
        var follow = newChar.transform.Find("FollowObject");
        if (follow == null)
            Debug.LogWarning($"[Companion] {newChar.name}에 FollowObject 없음!");

        targetObject = follow != null ? follow : newChar.transform;
        lookObject = newChar.transform;

        rb = rb ?? GetComponent<Rigidbody>();
        anim = anim ?? GetComponent<Animator>();

        cachedAnchorLocalPos = targetObject.localPosition;

        Debug.Log($"[Companion] Follow target → {targetObject.name}");
    }




    void Update() { Sm.HandleInput(); Sm.Update(); }
    void FixedUpdate() { Sm.PhysicsUpdate(); }

    // 전투 이벤트는 상태 전환만 담당 (행동은 BattleState가 함)
    void BattleStart(BattleZone zone)
    {
        isAttack = true;
        Sm.ChangeState(new BattleState(Sm));
    }
    void BattleClear(BattleZone zone)
    {
        isAttack = false;
        Sm.ChangeState(new CompanionFollowState(Sm));
    }

    // Talk 종료는 공용 헬퍼(상태/버튼에서 호출)
    public void ExitTalkMode()
    {
        if (ui) ui.Hide();
        if (chatUI) chatUI.SetActive(false);

        if (targetObject) targetObject.localPosition = cachedAnchorLocalPos;
        Cursor.lockState = cachedLockMode;
        Cursor.visible = cachedCursorVisible;

        isTalkMode = false;
        PlayerManager.Instance?.EnableInput(true);
    }
}
