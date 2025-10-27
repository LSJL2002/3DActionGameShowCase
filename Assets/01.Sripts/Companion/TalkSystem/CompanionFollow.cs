using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    private GameObject targetObject;
    private Vector3 initialOffset = new Vector3(0f, 1f, 1f);
    private Vector3 lookOffset = new Vector3(0f, 0.5f, 0f);
    private Vector3 targetLocalOffset; // 타겟의 로컬 공간에서 초기 오프셋을 저장

    public void Start()
    {
        targetObject = CompanionController.thisGO;
        if (targetObject == null) return;
        targetLocalOffset = targetObject.gameObject.transform.InverseTransformDirection(transform.position - targetObject.gameObject.transform.position);
        targetLocalOffset = initialOffset; // 타겟의 로컬 공간에서의 원하는 위치 오프셋 저장
        transform.position = targetObject.gameObject.transform.TransformPoint(targetLocalOffset); // OnEnable 시점에 초기 위치 설정
        LookAtTarget();
    }

    void LateUpdate()
    {
        if (targetObject == null) return;
        transform.position = targetObject.gameObject.transform.TransformPoint(targetLocalOffset);
        LookAtTarget();
    }

    // 타겟을 바라보도록 카메라 회전을 계산하고 적용하는 함수
    private void LookAtTarget()
    {
        // 카메라가 바라볼 월드 위치: 타겟의 중심 + lookOffset
        Vector3 lookAtPoint = targetObject.gameObject.transform.position + lookOffset;
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position, Vector3.up);
        transform.rotation = targetRotation;
    }
}