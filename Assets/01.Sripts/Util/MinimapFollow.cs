using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    // 플레이어 트랜스폼
    private Transform target;

    // 미니맵 카메라의 고정된 높이
    public float height = 50f;

    private void Awake()
    {
        target = PlayerManager.Instance.ActiveCharacter.transform;
    }

    // 모든 이동과 회전이 완료된 후 실행되도록 LateUpdate 사용
    void LateUpdate()
    {
        // 카메라 위치 (Position)
        Vector3 newPosition = target.position;
        newPosition.y = target.position.y + height; // 플레이어 위치보다 height만큼 위에 배치
        transform.position = newPosition; // 카메라 위치 업데이트

        // 카메라 회전 (Rotation)
        // X : 90f (위에서 아래로 바라봄)
        // Y : 0f (항상 북쪽을 향함)
        // Z : 0f (기울어지지 않음)
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public void Start()
    {
        PlayerManager.Instance.OnActiveCharacterChanged -= ChangeTarget;
        PlayerManager.Instance.OnActiveCharacterChanged += ChangeTarget;
    }

    // 카메라 Follow 대상을 변경하는 함수
    public void ChangeTarget(PlayerCharacter playerCharacter)
    {
        target = PlayerManager.Instance.ActiveCharacter.transform;
    }
}