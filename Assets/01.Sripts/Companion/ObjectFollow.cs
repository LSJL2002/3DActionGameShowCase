using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    [Header("FollowObject")]
    [SerializeField] private Transform targetObject;

    [Header("LookAtObject")]
    [SerializeField] private Transform lookObject;

    // 회전과 이동에 속도를 적용합니다.
    [Header("Speed")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("This.RigidBody")]
    [SerializeField] private Rigidbody rb;

    [Header("This.RigidBody")]
    [SerializeField] private GameObject talkUI;

    private void Update()
    {
        OnClickTarget();
    }

    private void FixedUpdate()
    {
        OnMove();
        FollowObject();
    }

    void FollowObject()
    {
        if (targetObject == null || rb == null /* || 플래이어에 현재상태 == 공격상태  (싱클톤확인) */) return;

        // Rigidbody를 이용한 부드러운 위치 이동
        Vector3 nextMove = Vector3.MoveTowards(rb.position, targetObject.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(nextMove);

        // 회전하는 방향을 정위하기 위해 로직 추가
        Vector3 dir = (lookObject.position - this.rb.position).normalized; // 바라볼 방향
        dir = Vector3.ProjectOnPlane(dir, Vector3.up); // 수평만 회전하게 만들다. (즉 y측만 회전)
        Quaternion look = Quaternion.LookRotation(dir, Vector3.up); // 바라보는 회전
        Quaternion nextRotation = Quaternion.RotateTowards(rb.rotation, look, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(nextRotation);
    }

    void OnMove()
    {
        anim.SetBool("isMove", (rb.velocity.magnitude > 0.1f) ? true : false);
    }

    void OnClickTarget()
    {
        if (targetObject == null) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            targetObject.position += targetObject.right * 1.03f;
            targetObject.position += targetObject.forward * 1.2f;
            talkUI.SetActive(true);
        }
            
    }
}


#region 이전코드
// 단순 위치 이동 (순간이동하는 느낌) 
//if (targetObject != null)
//{
//    this.transform.position = targetObject.position;
//    this.transform.rotation = targetObject.rotation;
//}

// 부드러운 위치 이동
//if ( targetObject == null ) return;
//this.transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime * moveSpeed);
//this.transform.rotation = Quaternion.Slerp(transform.rotation, targetObject.rotation, Time.deltaTime * rotationSpeed);

// Rigidbody를 이용한 부드러운 회전
//Quaternion nextRotation = Quaternion.RotateTowards(rb.rotation, targetObject.rotation, rotationSpeed * Time.deltaTime);
//rb.MoveRotation(nextRotation);

//// 회전하는 방향을 정위하기 위해 로직 추가
//Vector3 dir = (lookObject.position - this.rb.position).normalized; // 바라볼 방향
//dir = Vector3.ProjectOnPlane(dir, Vector3.up); // 수평만 회전하게 만들다. (즉 y측만 회전)
//Quaternion look = Quaternion.LookRotation(dir, Vector3.up); // 바라보는 회전
//Quaternion nextRotation = Quaternion.RotateTowards(rb.rotation, look, rotationSpeed * Time.deltaTime);
//rb.MoveRotation(nextRotation);

//// 새로운 이동 로직 
//// 타겟의 y축 위치를 무시하고 현재 y축 위치 유지
//Vector3 targetPos = targetObject.position;
//targetPos.y = rb.position.y; // 현재 높이 유지
//Vector3 toTarget = targetPos - rb.position;
//float dist = toTarget.magnitude;
//if (dist > 2f) // 2m보다 멀면 따라감
//{
//    // 타겟으로부터 2m 떨어진 지점까지 이동
//    Vector3 goal = targetPos - toTarget.normalized * 0.5f;

//    Vector3 nextMove = Vector3.MoveTowards(rb.position, goal, moveSpeed * Time.fixedDeltaTime);
//    rb.MovePosition(nextMove);
//}
//// 2~3m 사이면 그대로 둠(지터 방지)

//// 키가 눌렸을 때 targetObject 위치 바꾸기
//if (!targetObject) return;

//// G: 타겟의 "앞 방향"으로 3m 텔레포트 (로컬 기준)
//if (Input.GetKeyDown(KeyCode.G))
//    targetObject.position += targetObject.forward * 3f;

//// H: 월드 기준 +Z로 3m 이동 (로컬 아님)
//if (Input.GetKeyDown(KeyCode.H))
//    targetObject.position += new Vector3(0f, 0f, 3f);

//// J: 특정 좌표로 즉시 이동 (Y는 유지)
//if (Input.GetKeyDown(KeyCode.J))
//{
//    Vector3 p = targetObject.position;
//    targetObject.position = new Vector3(0f, p.y, 0f);
//}
#endregion
