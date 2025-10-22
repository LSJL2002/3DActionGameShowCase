using UnityEngine;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GaugeComponent : MonoBehaviour
{
    public enum GaugeState2
    {
        On, // 켜짐
        Off, // 꺼짐
        Awaken, // 각성
    }

    public GaugeState2 currentGaugeState { get; private set; } = GaugeState2.Off;

    [SerializeField] private Image defaultImage;
    [SerializeField] private Image waveImage;
    [SerializeField] private Image popImage;

    private float startY = -80f;  // 하단 기준
    private float waveDuration = 1f; // 웨이브 주기 시간(게이지 1개 오르락 내리락 한번 기준)

    // 이 객체를 생성한 Addressables 핸들을 저장할 변수
    private AsyncOperationHandle<GameObject> creationHandle;
    
    private Tweener waveTween; // 웨이브 연출을 저장할 변수
    private Sequence popGaugeSequence; // 웨이브 보이게하는 시퀀스를 저장할 변수

    private Color gaugeColor0 = new Color(1f, 1f, 1f, 0f); // 투명도0
    private Color gaugeColor1 = new Color(1f, 1f, 1f, 1f); // white 계열
    private Color gaugeColor2 = new Color(1f, 200f/255f, 0f, 1f); // yellow 계열
    private Color gaugeColor3 = new Color(1f, 0f, 1f, 1f); // red 계열

    private void OnEnable()
    {
        SetOriginGauge();
    }

    public void SetOriginGauge()
    {
        defaultImage.color = gaugeColor1;
        waveImage.color = gaugeColor0; // 웨이브 이미지 투명도 0으로 초기화
    }

    // Addressables 핸들을 설정하는 메서드 (생성 직후 호출됨)
    public void SetHandle(AsyncOperationHandle<GameObject> handle)
    {
        creationHandle = handle;
    }

    // 게이지 세팅
    public void SetGauge(GaugeState2 state)
    {
        float randomColor = Random.Range(20f / 255f, 100f / 255f); // 투명도 랜덤
        currentGaugeState = state;

        switch (state)
        {
            // 게이지 충전시
            case GaugeState2.On:

                defaultImage.color = gaugeColor2;
                waveImage.color = new Color(gaugeColor2.r, gaugeColor2.g, gaugeColor2.b, randomColor);
                break;

            // 게이지 비활성시
            case GaugeState2.Off:

                SetOriginGauge();
                break;

            // 게이지 사용시
            case GaugeState2.Awaken:

                defaultImage.color = gaugeColor3;
                waveImage.color = new Color(gaugeColor3.r, gaugeColor3.g, gaugeColor3.b, randomColor);
                break;
        }
    }

    public void StartWaveEffect(float delay)
    {
        // 기존 트윈 완전 종료
        if (waveTween != null && waveTween.IsActive()) waveTween.Kill();

        // RectTransform 초기화 (즉시 반영)
        waveImage.rectTransform.anchoredPosition = new Vector3(0, startY);

        float randomEndY = Random.Range(-80, -40f); // 웨이브 높이는 랜덤 값

        waveTween = waveImage.rectTransform
            .DOAnchorPosY(randomEndY, waveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay);
    }

    // 웨이브 애니메이션 시작하기 전 연출 (Pop 효과)
    public void PopGaugeEffect()
    {
        popGaugeSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
        popGaugeSequence.Append(popImage.DOFade(0.2f, 0.2f)); // PopImage 나타나는 트윈 추가
        popGaugeSequence.Append(popImage.DOFade(0f, 0.1f)); // PopImage 사라지는 트윈 추가
        popGaugeSequence = null;

        SetGauge(GaugeState2.On);
    }

    // 객체가 파괴될 때 Addressables 인스턴스를 해제하는 메서드
    public void ReleaseInstance()
    {
        if (creationHandle.IsValid())
        {
            // DOTween 애니메이션 중단
            waveImage.DOKill();
            // 실제 Addressables 해제
            Addressables.ReleaseInstance(creationHandle);
        }
    }
}