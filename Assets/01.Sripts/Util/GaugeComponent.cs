using UnityEngine;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GaugeComponent : MonoBehaviour
{
    [SerializeField] private Image emptyImage;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image popImage;
    public bool isFillImage = false;

    private float startY = -80f;  // 하단 기준
    private float endY = -50f;  // 상단 기준
    private float waveDuration = 1f; // 웨이브 주기 시간(게이지 1개 오르락 내리락 한번 기준)

    // 이 객체를 생성한 Addressables 핸들을 저장할 변수
    private AsyncOperationHandle<GameObject> creationHandle;
    // 웨이브 애니메이션을 제어하는 Tweener
    private Tweener waveTween;
    Sequence waveSequence;

    private void OnEnable()
    {
        emptyImage.color = Color.white; // 활성화시 흰색으로 초기화

        
        fillImage.color = new Color(255f/255f, 200f/255f, 0f/255f, 0f); // 채움색깔은 '00'색에 투명도 0
    }

    // Addressables 핸들을 설정하는 메서드 (생성 직후 호출됨)
    public void SetHandle(AsyncOperationHandle<GameObject> handle)
    {
        creationHandle = handle;
    }

    public void SetFill()
    {
        emptyImage.color = Color.yellow; // 게이지 채워질 때 00색으로 변경
        isFillImage = true;
    }

    public void StartWaveAnimation(float delay)
    {
        // 기존 트윈 완전 종료
        if (waveTween != null && waveTween.IsActive()) waveTween.Kill();

        // RectTransform 초기화 (즉시 반영)
        fillImage.rectTransform.anchoredPosition = new Vector3(0, startY);

        float randomEndY = Random.Range(-80, -40f); // 웨이브 최상단은 랜덤 값

        // 절대 좌표 기반 트윈
        waveTween = fillImage.rectTransform
            .DOAnchorPosY(randomEndY, waveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay);
    }

    // 웨이브 애니메이션을 시각화할 때 호출 (알파값 0에서 n으로 변경)
    public void VisualizeWaveAnimation()
    {
        float randomColor = Random.Range(20f / 255f, 100f / 255f);
        Color setFillImageColor = new Color(255f / 255f, 200f / 255f, 0f / 255f, randomColor);

        waveSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
        waveSequence.Append(popImage.DOFade(0.2f, 0.2f)); // PopImage 나타나는 트윈 추가
        waveSequence.Append(popImage.DOFade(0f, 0.1f)); // PopImage 사라지는 트윈 추가
        waveSequence.Append(fillImage.DOColor(setFillImageColor, 0.1f)); // PopImage 사라진 후 fillImage 알파값을 n으로 변경
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