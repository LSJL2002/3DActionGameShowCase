using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFollow : MonoBehaviour
{
    [Header("FollowObject")]
    [SerializeField] public Transform targetObject;

    [Header("LookAtObject")]
    [SerializeField] public Transform lookObject;

    // 회전과 이동에 속도를 적용합니다.
    [Header("Speed")]
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public float rotationSpeed = 360f;

    [Header("Animation")]
    [SerializeField] public Animator anim;

    [Header("This.RigidBody")]
    [SerializeField] public Rigidbody rb;

    [Header("TalkUI")]
    [SerializeField] public GameObject talkUI;
    [SerializeField] public Button stateBtn;
    [SerializeField] public Button inventoryBtn;
    [SerializeField] public Button talkBtn;
    
    [Header("파티클")]
    [SerializeField] private GameObject moveFx;   // 파티클이 붙은 오브젝트(프리팹 인스턴스)
    [SerializeField] private float moveSpeedThreshold = 0.1f; // 이동 판정 기준

    // G키를 다시 누렀을 때 원상복귀에 필요한 변수
    private bool isTalkMode = false; 
    private Vector3 cachedAnchorLocalPos; // 캐릭터 중심으로 처음에 고정한 오브젝트 위치
    private CursorLockMode cachedLockMode; // 커서
    private bool cachedCursorVisible; // 커서가 보이고 안보이고하는 bool값

    private void Awake()
    {
        if (targetObject != null)
            cachedAnchorLocalPos = targetObject.localPosition; // 최초 위치세팅 저장
    }

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
        bool isMoving = rb != null && rb.velocity.sqrMagnitude > (moveSpeedThreshold * moveSpeedThreshold);

        // 애니메이션
        anim.SetBool("isMove", isMoving);

        // 파티클 ON/OFF
        if (moveFx != null) moveFx.SetActive(isMoving);
    }

    void OnClickTarget()
    {
        if (targetObject == null) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isTalkMode)
            {
                Vector3 localOffset = new Vector3(1.03f, 0f, 1.2f); // x=Right, z=Forward
                targetObject.localPosition = cachedAnchorLocalPos + localOffset;

                // 키 클릭 시 상태
                StartCoroutine(ShowTalkAndPauseAfterDelay(1.2f));
            }
            else
            {
                ExitTalkMode();
            }
        }
    }

    IEnumerator ShowTalkAndPauseAfterDelay(float delay)
    {
        // 현재 커서 상태를 저장 (나중에 복원할 때 사용)
        cachedLockMode = Cursor.lockState;
        cachedCursorVisible = Cursor.visible;

        yield return new WaitForSeconds(delay);

        talkUI.SetActive(true); // UI 표시

        // UI 조작을 위해 커서 해제
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isTalkMode = true;
    }

    void ExitTalkMode()
    {
        // UI 닫기 + 정지 해제
        talkUI.SetActive(false);

        // 위치 원복
        targetObject.localPosition = cachedAnchorLocalPos;

        // 커서 상태 복원
        Cursor.lockState = cachedLockMode; // ShowTalkAndPauseAfterDelay() 메서드에 저장한 상태로 복원
        Cursor.visible = cachedCursorVisible; // 원래 안 보이던 상태로 돌림

        isTalkMode = false;
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

//void OnClickTarget()
//{
//    if (targetObject == null) return;

//    if (Input.GetKeyDown(KeyCode.G))
//    {
//        targetObject.position += targetObject.right * 1.03f;
//        targetObject.position += targetObject.forward * 1.2f;
//        StartCoroutine(ShowTalkAndPauseAfterDelay(1.5f));
//    }

//}

//targetObject.position += targetObject.right * 1.03f;
//targetObject.position += targetObject.forward * 1.2f;.

// anim.SetBool("isMove", rb.velocity.magnitude > 0.1f);
#endregion
