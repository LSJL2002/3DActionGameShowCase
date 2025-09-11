using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class UnityChanMove : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float sprint = 10f;
    public float jumpForce = 5f;

    float finalSpeed;
    bool run;
    Vector2 input;

    Animator _animator;
    Rigidbody _rb;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        // Rigidbody 기본 세팅
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // 입력 처리
        run = Input.GetKey(KeyCode.LeftShift);
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 애니메이션 블렌딩
        float percent = (run ? 1f : 0.5f) * Mathf.Clamp01(input.magnitude);
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (_rb == null) return; // 안전망

        finalSpeed = run ? sprint : speed;

        // 월드 기준 이동
        Vector3 worldMove = new Vector3(input.x, 0f, input.y);
        if (worldMove.sqrMagnitude > 1e-4f)
            worldMove.Normalize();

        // 속도 적용 (y는 유지)
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

        // TODO: 점프는 별도 플래그 전달해서 여기서 처리
    }
}
