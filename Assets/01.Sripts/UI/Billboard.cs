using UnityEngine;
using UnityEngine.SceneManagement;

// 캐릭터 이름 Text가 카메라에 정면으로 비추게 하기 위한 코드
public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void OnEnable()
    {
        if (Camera.main != null)
        {
            // 메인 카메라의 Transform을 찾기
            mainCameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        // Canvas가 항상 카메라를 바라보도록 회전
        // LookAt() 함수는 지정된 대상(카메라)을 바라보게 함
        // Vector3.up은 오브젝트의 위쪽 방향이 월드의 위쪽과 일치하게 함
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}