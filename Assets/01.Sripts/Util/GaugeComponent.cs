using UnityEngine;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GaugeComponent : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    public bool isActiveFillImage = false;
    [SerializeField] private float waveAmplitude = 10f; // 파동의 진폭
    [SerializeField] private float waveFrequency = 0.5f; // 파동의 주파수

    // 이 객체를 생성한 Addressables 핸들을 저장할 변수
    private AsyncOperationHandle<GameObject> creationHandle;

    // Addressables 핸들을 설정하는 메서드 (생성 직후 호출됨)
    public void SetHandle(AsyncOperationHandle<GameObject> handle)
    {
        creationHandle = handle;
        fillImage.gameObject.SetActive(false); // 초기에는 비활성화
    }

    public void SetFill()
    {
        fillImage.gameObject.SetActive(true);
        isActiveFillImage = true;
    }

    public void StartWaveAnimation(float delay)
    {
        fillImage.rectTransform
            .DOLocalMoveY(waveAmplitude, waveFrequency)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(delay);
    }

    // 객체가 파괴될 때 Addressables 인스턴스를 해제하는 메서드
    public void ReleaseInstance()
    {
        if (creationHandle.IsValid())
        {
            // DOTween 애니메이션 중단
            fillImage.DOKill();
            // 실제 Addressables 해제
            Addressables.ReleaseInstance(creationHandle);
        }
    }
}