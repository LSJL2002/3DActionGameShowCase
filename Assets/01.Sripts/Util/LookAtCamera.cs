using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AimConstraint aimConstraint;

    public @PlayerInput playerInput;

    private void Awake()
    {
        canvasGroup.alpha = 0f;

        // 메인 카메라를 플레이어에서 가져옴
        Transform camTransform = PlayerManager.Instance.camera.MainCamera;

        // 새로운 소스 생성
        ConstraintSource source = new ConstraintSource();

        // 소스 정보 할당
        source.sourceTransform = camTransform;
        source.weight = 1.0f;

        // 완성된 소스를 세팅
        aimConstraint.SetSource(0, source);

        playerInput = new @PlayerInput();

        playerInput.Player.Camera.performed += SetRockOn; // 키입력('F')에 OnGameUI 함수 구독
        playerInput.Player.Enable(); // Player 액션 맵 활성화 (다시 입력을 알 수 있도록 켜둠)
    }

    private void SetRockOn(InputAction.CallbackContext context)
    {
        canvasGroup.alpha = (canvasGroup.alpha == 0f) ? 1f : 0f;
    }

    private void OnDestroy()
    {
        playerInput.Player.Camera.performed -= SetRockOn; // 키입력('F')에 함수 구독해제
        playerInput.Player.Disable(); // Player 액션 맵 활성화 (입력 대기가 필요없으므로 꺼둠)
    }
}