using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class LookAtCamera : MonoBehaviour
{

    private AimConstraint aimConstraint;

    void Start()
    {
        // 메인 카메라를 플레이어에서 가져옴
        Transform cam = PlayerManager.Instance.camera.MainCamera;

        // 에임컨스트레인트 컴포넌트 가져오기
        aimConstraint = GetComponent<AimConstraint>();

        // 새로운 소스 생성
        ConstraintSource source = new ConstraintSource();

        // 소스 정보 할당
        source.sourceTransform = cam;
        source.weight = 1.0f;

        // 완성된 소스를 세팅
        aimConstraint.SetSource(0, source);
    }
}