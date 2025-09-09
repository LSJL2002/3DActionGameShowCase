using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class UnityChanMove : MonoBehaviour
{
    public float speed = 2f;
    public float sprint = 10f;
    public float jumpForce = 5f;

    float finalSpeed;
    bool run;
    Vector2 input;

    Animator _animator;
    Rigidbody _rb;
    Camera _cam;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;

        // Rigidbody 기본 세팅
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Y는 자유
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // 입력만 Update에서
        run = Input.GetKey(KeyCode.LeftShift);
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 애니메이션 (입력 크기 기반)
        float percent = (run ? 1f : 0.5f) * Mathf.Clamp01(input.magnitude);
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (_rb == null) return; // 안전망

        finalSpeed = run ? sprint : speed;

        // 카메라 기준 이동 벡터
        Vector3 camF = _cam ? Vector3.Scale(_cam.transform.forward, new Vector3(1, 0, 1)).normalized : Vector3.forward;
        Vector3 camR = _cam ? _cam.transform.right : Vector3.right;
        Vector3 worldMove = (camF * input.y + camR * input.x);
        if (worldMove.sqrMagnitude > 1e-4f) worldMove.Normalize();

        // 수평 속도 적용, y는 유지
        Vector3 vel = _rb.velocity;
        vel.x = worldMove.x * finalSpeed;
        vel.z = worldMove.z * finalSpeed;
        _rb.velocity = vel;

        // 회전 (입력 있을 때만)
        if (worldMove.sqrMagnitude > 1e-4f)
        {
            Quaternion target = Quaternion.LookRotation(worldMove);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, target, 10f * Time.fixedDeltaTime));
        }

        // 점프 입력은 Update에서 눌러도 여기서 처리하고 싶다면 플래그로 전달해도 됨
        if (_pendingJump && IsGrounded())
        {
            vel = _rb.velocity;
            vel.y = 0f; // 일관된 점프
            _rb.velocity = vel;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        _pendingJump = false;
    }

    bool _pendingJump;
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
            _pendingJump = true; // FixedUpdate에서 처리
    }

    bool IsGrounded()
    {
        // 캡슐 바닥 기준 간단 레이캐스트
        float dist = GetComponent<CapsuleCollider>().bounds.extents.y + 0.05f;
        return Physics.Raycast(transform.position, Vector3.down, dist);
    }
}
