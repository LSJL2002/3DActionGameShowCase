using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform targetObject;
    private Vector3 initialOffset = new Vector3(0f, 2f, 4f);
    private Vector3 lookOffset = new Vector3(0f, 1.15f, 0f);
    private Vector3 targetLocalOffset; // 타겟의 로컬 공간에서 초기 오프셋을 저장

    void OnEnable()
    {
        PlayerManager.Instance.OnActiveCharacterChanged -= SetTarget;
        PlayerManager.Instance.OnActiveCharacterChanged += SetTarget;

        SetTarget(PlayerManager.Instance.ActiveCharacter);
    }

    void LateUpdate()
    {
        if (targetObject == null) return;
        transform.position = targetObject.TransformPoint(targetLocalOffset);
        LookAtTarget();
    }

    private void SetTarget(PlayerCharacter playerCharacter)
    {
        targetObject = PlayerManager.Instance.ActiveCharacter.transform;
        if (targetObject == null) return;
        targetLocalOffset = targetObject.InverseTransformDirection(transform.position - targetObject.position);
        targetLocalOffset = initialOffset; // 타겟의 로컬 공간에서의 원하는 위치 오프셋 저장
        transform.position = targetObject.TransformPoint(targetLocalOffset); // OnEnable 시점에 초기 위치 설정
        LookAtTarget();
    }

    // 타겟을 바라보도록 카메라 회전을 계산하고 적용하는 함수
    private void LookAtTarget()
    {
        // 카메라가 바라볼 월드 위치: 타겟의 중심 + lookOffset
        Vector3 lookAtPoint = targetObject.position + lookOffset;
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position, Vector3.up);
        transform.rotation = targetRotation;
    }
}