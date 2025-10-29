using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenCamera : MonoBehaviour
{
    public Transform target; // 중심으로 할 객체
    public float distance = 10f; // 카메라와 객체 사이의 거리
    public float heightOffset = 2f; // 카메라와 객체 사이의 높이

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialTargetPosition;
    private Animator targetAnimator; // 타겟의 Animator 컴포넌트

    private void Start()
    {
        // 초기 카메라 각도 및 위치 설정
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialTargetPosition = target.position;

        // 타겟의 Animator 컴포넌트 가져오기
        if (target != null)
        {
            targetAnimator = target.GetComponent<Animator>();
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 카메라 위치와 회전 업데이트 (타겟 기준)
        Vector3 offset = new Vector3(0f, heightOffset, 0f);
        transform.position = target.position + offset - transform.rotation * Vector3.forward * distance;
        transform.LookAt(target.position + offset);
    }

    private void ResetCamera()
    {
        distance = Vector3.Distance(initialPosition, target.position);
        heightOffset = initialPosition.y - target.position.y;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        target.position = initialTargetPosition;

        // 타겟 애니메이션 초기화
        if (targetAnimator != null)
        {
            targetAnimator.Play(targetAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
        }
    }
}