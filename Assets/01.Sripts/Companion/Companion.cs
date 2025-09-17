using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// 조력자(Companion)의 본체.
/// - 현재 상태 보유 및 업데이트 위임
/// - 공통 데이터(플레이어 참조, 거리/속도 등) 제공
/// - Rigidbody 이동을 위한 구성/헬퍼 제공
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Companion : MonoBehaviour
{
    [Header("수직(Y) 고정 옵션")]
    public bool lockInitialY = true;
    private float _initialY;
    public float LockedY => lockInitialY ? _initialY : rb.position.y;

    [Header("Follow 대상")]
    [Tooltip("따라갈 플레이어(또는 타겟) Transform")]
    public Transform player;

    [Header("거리 파라미터")]
    [Tooltip("이 거리 이하이면 멈춤(Idle로 전환)")]
    public float desiredFollowDistance = 2.0f;
    [Tooltip("이 거리 이상이면 Follow 시작")]
    public float followStartDistance = 4.0f;

    [Header("이동 파라미터")]
    [Tooltip("최대 이동 속도(m/s)")]
    public float moveSpeed = 4.0f;
    [Tooltip("목표를 바라보는 회전 스무딩")]
    public float turnLerp = 10.0f;

    [Header("물리 구성")]
    [Tooltip("수평면에서만 따라가고 싶으면 체크(상하(y) 속도 제거)")]
    public bool constrainToXZPlane = true;

    [Header("정지 버퍼(진입 히스테리시스)")]
    [Tooltip("desiredFollowDistance보다 이만큼 더 여유가 있어야 다시 움직입니다.")]
    public float stopBuffer = 0.2f;

    // 컴포넌트
    [HideInInspector] public Rigidbody rb;

    // FSM
    private PetStateMachine _fsm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // 권장 물리 설정(취향대로 조정 가능)
        rb.interpolation = RigidbodyInterpolation.Interpolate; // 부드러운 이동
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // 필요 시 회전 제약: rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _initialY = transform.position.y;
        if (lockInitialY) rb.constraints |= RigidbodyConstraints.FreezePositionY;
    }

    private void Start()
    {
        // 상태머신 초기화(Idle로 시작)
        _fsm = new PetStateMachine();
        _fsm.Initialize(new IdleState(this));
    }

    private void Update()
    {
        // 로직/판정(Update 계열)
        _fsm.TickUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // 실제 이동/물리(FixedUpdate 계열)
        _fsm.TickFixedUpdate(Time.fixedDeltaTime);
    }

    /// <summary>외부/상태에서 상태 전환을 요청할 때 사용</summary>
    public void ChangeState(PetState next) => _fsm.Change(next);

    /// <summary>
    /// Rigidbody.MovePosition을 이용한 이동(물리 친화적).
    /// 상태에서 공통으로 쓰도록 헬퍼로 제공.
    /// </summary>
    public void MoveTowards(Vector3 targetPos, float speed, float dt)
    {
        //Vector3 current = rb.position;
        //Vector3 to = targetPos - current;
        //if (constrainToXZPlane) to.y = 0f;              // 수평면 이동
        //Vector3 dir = to.sqrMagnitude > 0.0001f ? to.normalized : Vector3.zero;
        //Vector3 next = current + dir * speed * dt;
        //rb.MovePosition(next);

        //// 간단 회전(선택): 이동 방향을 부드럽게 바라보게
        //if (dir.sqrMagnitude > 0.0001f)
        //{
        //    Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        //    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnLerp * dt));
        //}

        Vector3 current = rb.position;
        Vector3 to = targetPos - current;
        if (constrainToXZPlane) to.y = 0f;
        Vector3 dir = to.sqrMagnitude > 0.0001f ? to.normalized : Vector3.zero;

        Vector3 next = current + dir * speed * dt;
        next.y = current.y;

        // 플레이어 최소 거리 보장(선택적 마지막 안전장치)
        if (player)
        {
            Vector3 fromPlayerNext = next - player.position;
            fromPlayerNext.y = 0f;
            float d = fromPlayerNext.magnitude;
            if (d < desiredFollowDistance)
            {
                // 원둘레에 투영해 최소 거리 유지
                next = player.position + fromPlayerNext.normalized * desiredFollowDistance;
            }
        }

        rb.MovePosition(next);

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnLerp * dt));
        }
    }

    private void OnValidate()
    {
        // 에디터에서 값 보호
        followStartDistance = Mathf.Max(followStartDistance, 0f);
        desiredFollowDistance = Mathf.Clamp(desiredFollowDistance, 0f, followStartDistance);
        moveSpeed = Mathf.Max(moveSpeed, 0f);
        turnLerp = Mathf.Max(turnLerp, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (!player) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, desiredFollowDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, followStartDistance);
    }
}
