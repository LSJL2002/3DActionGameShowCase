using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float offsetDistance = 1.5f;
    [SerializeField] private float yOffset = -1f;

    private Transform camTransform; // 메인 카메라의 트랜스폼
    public PlayerInputs playerInput; // 인풋 시스템 액션 맵
    private Transform monsterTransform;

    private void Awake()
    {
        monsterTransform = transform.parent;
        camTransform = PlayerManager.Instance._camera.MainCamera;

        // 인풋 시스템 설정
        playerInput = new PlayerInputs();
        playerInput.Player.Camera.performed += SetRockOn; // 키입력('F')에 함수 구독
        playerInput.Player.Enable(); // Player 액션 맵 활성화

        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnMonsterDie, DisableRockOn); // 구독해제
        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnMonsterDie, DisableRockOn); // 구독
    }

    private void LateUpdate()
    {
        if (camTransform == null || monsterTransform == null)
        {
            return;
        }

        transform.LookAt(camTransform);

        // 몬스터의 현재 위치 (부모의 위치)
        Vector3 monsterCenter = monsterTransform.position;

        // 몬스터 중심에서 카메라(플레이어)를 향하는 수평 방향 벡터 계산
        Vector3 directionToCamera = camTransform.position - monsterCenter;
        directionToCamera.y = 0; // Y축 변화는 무시 (수평 방향만 사용)
        directionToCamera.Normalize(); // 크기를 1로 만듦

        // 몬스터 중심 위치를 기준으로 플레이어 방향으로 offsetDistance만큼 밀어냄
        Vector3 newPosition = monsterCenter + (directionToCamera * offsetDistance);

        // Y축 위치 보정 (몬스터 머리 위 등으로 띄움)
        newPosition.y = monsterCenter.y + yOffset;

        // 록온 이미지 오브젝트의 최종 위치 설정
        transform.position = newPosition;
    }

    private void SetRockOn(InputAction.CallbackContext context)
    {
        // 알파값을 0과 1로 토글 (록온 이미지 껐다 켜기)
        canvasGroup.alpha = (canvasGroup.alpha == 0f) ? 1f : 0f;
    }

    private void DisableRockOn(BattleZone zone)
    {
        // 몬스터가 죽으면 록온 이미지 끄기
        canvasGroup.alpha = 0f;

        // 인풋 시스템 구독 해제 및 비활성화 (시체에 록온 못하게)
        playerInput.Player.Camera.performed -= SetRockOn;
        playerInput.Player.Disable();
    }

    private void OnDestroy()
    {
        // 인풋 시스템 구독 해제 및 비활성화
        playerInput.Player.Camera.performed -= SetRockOn;
        playerInput.Player.Disable();
    }
}