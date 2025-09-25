using System.Collections;
using System.Collections.Generic;
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

    void OnEnable()
    {
        BattleManager.OnBattleStart += OnBattleStart;
        BattleManager.OnBattleClear += OnBattleClear;
    }
    void OnDisable()
    {
        BattleManager.OnBattleStart -= OnBattleStart;
        BattleManager.OnBattleClear -= OnBattleClear;
    }

    void Update() { Sm.HandleInput(); Sm.Update(); }
    void FixedUpdate() { Sm.PhysicsUpdate(); }

    // 전투 이벤트는 상태 전환만 담당 (행동은 BattleState가 함)
    void OnBattleStart(BattleZone zone)
    {
        isAttack = true;
        Sm.ChangeState(new BattleState(Sm));
    }
    void OnBattleClear(BattleZone zone)
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
