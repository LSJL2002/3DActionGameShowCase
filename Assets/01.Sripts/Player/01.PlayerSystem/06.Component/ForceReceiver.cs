using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    //캐릭터가 받아야 할 힘을 모아놓은 계산기
    //중력, 넉백, 점프 같은걸 더 자연스럽게 만들어줌
    //그 힘을 실제로 적용시키는건 캐릭터컨트롤러의 Move

    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float drag = 0.3f;  // 수평 감속

    private Vector3 impact;               // 외부 힘 (X/Z + Y)
    private Vector3 dampingVelocity;      // SmoothDamp 내부용
    private float verticalVelocity;       // Y축 힘
    private float holdTargetY = 0f;       // 공중 유지 목표 Y
    private bool holdActive = false;      // 공중 유지 상태
    private float holdEndTime;            // 공중 유지 종료 시간
    private float holdLerpSpeed = 8f;     // Y축 보간 속도

    private bool freezeVertical = false;  // 공중 유지 및 루트모션 제어
    public bool FreezeVertical
    {
        get => freezeVertical;
        set
        {
            freezeVertical = value;
            if (animator != null)
                animator.applyRootMotion = !freezeVertical; // 루트모션 자동 ON/OFF
        }
    }

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;


    private void Awake()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (holdActive && controller != null)
        {
            // 1️⃣ 수평 힘 감소
            Vector3 horizontalImpact = new Vector3(impact.x, 0f, impact.z);
            horizontalImpact = Vector3.SmoothDamp(horizontalImpact, Vector3.zero, ref dampingVelocity, drag);
            impact.x = horizontalImpact.x;
            impact.z = horizontalImpact.z;

            // 2️⃣ Y축 목표 위치로 부드럽게 이동
            float currentY = controller.transform.position.y;
            float newY = Mathf.Lerp(currentY, holdTargetY, Time.deltaTime * holdLerpSpeed);
            float yMove = newY - currentY;
            float maxStep = 5f * Time.deltaTime; // 안전 클램프
            yMove = Mathf.Clamp(yMove, -maxStep, maxStep);

            // 3️⃣ 실제 이동
            Vector3 horizontalDisplacement = new Vector3(horizontalImpact.x, 0f, horizontalImpact.z) * Time.deltaTime;
            controller.Move(horizontalDisplacement + new Vector3(0f, yMove, 0f));

            // 4️⃣ 공중 유지 종료 체크
            if (Time.time >= holdEndTime)
                EndVerticalHold();
        }
        else
        {
            // 일반 중력 처리
            if (controller != null)
            {
                if (controller.isGrounded)
                    verticalVelocity = -0.1f; // 착지 폴짝 방지
                else
                    verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            // 수평 힘 감속
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

            // 이동 적용
            if (controller != null && controller.enabled && gameObject.activeInHierarchy)
                controller.Move(Movement * Time.deltaTime);

            // 착지 시 Y축 초기화
            if (controller != null && controller.isGrounded && verticalVelocity <= 0f)
                impact.y = 0f;
        }
    }

    // ===================== 외부 API =====================
    public void Reset()
    {
        verticalVelocity = 0;
        impact = Vector3.zero;
        holdActive = false;
    }


    // 넉백 / 외부 힘 적용
    public void AddForce(Vector3 force, bool horizontalOnly = false)
    {
        if (horizontalOnly) force.y = 0;
        impact += force;
    }

    // 점프용 순간 힘
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }


    // ===== public API: 안전한 공중 유지 시작 =====
    // 공중 유지 연출 시작
    public void BeginVerticalHold(float heightOffset, float duration)
    {
        if (controller == null) return;

        verticalVelocity = 2f;
        holdTargetY = controller.transform.position.y + heightOffset;
        holdEndTime = Time.time + duration;
        holdActive = true;
        FreezeVertical = true; // 루트모션 OFF + 중력 정지
    }

    // 공중 유지 종료
    public void EndVerticalHold()
    {
        holdActive = false;
        verticalVelocity = 0f;
        FreezeVertical = false; // 루트모션 복귀 + 중력 활성화
    }

    // 공중 목표 Y 지정 (직접 제어용)
    public void SetTargetY(float y)
    {
        FreezeVertical = true;
        holdActive = true;
    }

    // 즉시 Y를 목표 위치로 올리고 끝까지 고정
    public void BeginVerticalHoldImmediate(float heightOffset, float duration)
    {
        if (controller == null) return;

        Vector3 pos = controller.transform.position;
        pos.y += heightOffset;
        controller.transform.position = pos;   // 바로 올림

        holdTargetY = pos.y;
        holdEndTime = Time.time + duration;
        holdActive = true;

        verticalVelocity = 0f;
        FreezeVertical = true; // 루트모션 끄고 중력 무효
    }
}
