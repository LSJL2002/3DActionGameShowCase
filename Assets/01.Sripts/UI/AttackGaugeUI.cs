using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AttackGaugeUI : UIBase
{
    public enum GaugeState1
    {
        Fill, // 충전중
        Full, // 만땅
        Awaken, // 사용중
    }
    
    private GaugeState1 currentGaugeState = GaugeState1.Fill;

    [SerializeField] CanvasGroup attackGaugeCanvasGroup;
    [SerializeField] private GameObject gaugeContainer;
    [SerializeField] private List<GaugeComponent> fillGauges = new List<GaugeComponent>();
    [SerializeField] private string gaugePrefabAddress = "Gauge_Mask";
    [SerializeField] private ModuleData_Yuki moduleData_Yuki;
    
    private float totalConsumeDuration = 10f;
    private Sequence consumeSequence;

    protected async override void Awake()
    {
        base.Awake();

        // 초기에 최대 게이지 개수만큼 추가(빈칸 상태)
        for (int i = 0; i < moduleData_Yuki.AwakenGaugeCost; i++)
        {
            await AddGaugeAsync();
        }

        // 모든 게이지 애니메이션 시작 (알파값 0상태라 안보임)
        StartGaugeAnimation();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => 
        {
            attackGaugeCanvasGroup.DOFade(1f, 1f);
        });
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 구독해제
        PlayerManager.Instance.Attr.AwakenGauge.OnChanged -= UpdateGauge;
        PlayerManager.Instance.Attr.AwakenGauge.OnFull -= FullGauge;
        PlayerManager.Instance.Attr.AwakenGauge.OnUsed -= UseGauge;
        PlayerManager.Instance.OnActiveCharacterChanged -= OnActiveGaugeUI;

        // 구독
        PlayerManager.Instance.Attr.AwakenGauge.OnChanged += UpdateGauge;
        PlayerManager.Instance.Attr.AwakenGauge.OnFull += FullGauge;
        PlayerManager.Instance.Attr.AwakenGauge.OnUsed += UseGauge;
        PlayerManager.Instance.OnActiveCharacterChanged += OnActiveGaugeUI;
    }

    // 캐릭터 변경시 UI 숨기고 켜는 함수
    public void OnActiveGaugeUI(PlayerCharacter playerCharacter)
    {
        switch (playerCharacter.CharacterType)
        {
            case CharacterType.Yuki:
                attackGaugeCanvasGroup.DOFade(1f, 1f);
                break;
            case CharacterType.Aoi:
                attackGaugeCanvasGroup.DOFade(0f, 1f);
                break;
            case CharacterType.Mika:
                attackGaugeCanvasGroup.DOFade(0f, 1f);
                break;
        }
    }

    // 게이지핸들 로드 함수 (어드레서블 로드 및 List 관리 방식)
    // UniTask : 이 함수가 비동기 함수임을 나타내는 반환 타입
    // GaugeComponent : 이 함수가 성공적으로 완료되었을 때 반환하는 값의 타입(Class명)
    public async UniTask<GaugeComponent> AddGaugeAsync()
    {
        var newHandle = Addressables.InstantiateAsync(gaugePrefabAddress, gaugeContainer.transform);
        await newHandle;

        // 성공 시 처리
        if (newHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GaugeComponent gaugeComp = newHandle.Result.GetComponent<GaugeComponent>();
            gaugeComp.SetHandle(newHandle); // 생성된 핸들을 설정
            fillGauges.Add(gaugeComp); // 리스트에 추가
            return gaugeComp; // GaugeComponent 객체를 반환
        }
        return null;
    }

    // 웨이브 연출 시작 함수
    public void StartGaugeAnimation()
    {
        for (int i = 0; i < fillGauges.Count; i++)
        {
            fillGauges[i].StartWaveEffect(i * 0.1f);
        }
    }

    // 게이지 업데이트 함수
    public void UpdateGauge(float amount)
    {
        if (currentGaugeState == GaugeState1.Fill)
        {
            for (int i = 0; i < amount; i++)
            {
                if (fillGauges[i].currentGaugeState == GaugeComponent.GaugeState2.Off)
                {
                    fillGauges[i].SetGauge(GaugeComponent.GaugeState2.On);
                }
            }
        }
    }

    // 게이지 Full 상태변경 함수
    public void FullGauge()
    {
        currentGaugeState = GaugeState1.Full; // 상태 변경
        foreach (var comp in fillGauges)
        {
            // fillImage 알파값을 n으로 변경
            comp.PopGaugeEffect();
        }
    }

    // 게이지 사용시 호출 (각성상태돌입)
    public void UseGauge()
    {
        currentGaugeState = GaugeState1.Awaken; // 상태 변경
        UseGauge(currentGaugeState);

        consumeSequence?.Kill();
        consumeSequence = StartConsumeSequence(totalConsumeDuration);
    }

    // 게이지 세팅 함수
    public void UseGauge(GaugeState1 gaugeState1)
    {
        switch (gaugeState1)
        {
            // 초기화
            case GaugeState1.Fill:

                foreach (var comp in fillGauges)
                {
                    comp.SetGauge(GaugeComponent.GaugeState2.Off);
                    comp.SetOriginGauge();
                }
                break;

            // 각성상태
            case GaugeState1.Awaken:

                foreach (var comp in fillGauges)
                {
                    comp.SetGauge(GaugeComponent.GaugeState2.Awaken);
                }

                currentGaugeState = GaugeState1.Awaken; // 상태 변경
                break;
        }
    }

    // 게이지를 뒤에서부터 순차적으로 끄는 메서드(Dotween)
    private Sequence StartConsumeSequence(float duration)
    {
        // 전체 소모 시간을 게이지 개수로 나누어 게이지 하나당 소모 간격을 계산
        float intervalPerGauge = duration / fillGauges.Count;

        // 새로운 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 인덱스를 뒤에서부터 순회하며 시퀀스에 작업을 추가합니다.
        for (int i = fillGauges.Count - 1; i >= 0; i--)
        {
            GaugeComponent gauge = fillGauges[i];

            // 게이지 끄는 작업 추가
            // AppendInterval(intervalPerGauge): 다음 작업을 실행하기 전에 'intervalPerGauge' 시간만큼 대기
            // AppendCallback: 대기 후 즉시 실행할 동작을 추가
            sequence.AppendInterval(intervalPerGauge)
                    .AppendCallback(() =>
                    {
                        if (gauge.currentGaugeState == GaugeComponent.GaugeState2.Awaken)
                        {
                            gauge.SetGauge(GaugeComponent.GaugeState2.Off);
                        }
                    });
        }

        // 전체 시퀀스가 완료된 후 실행할 동작 추가
        sequence.OnComplete(() =>
        {
            currentGaugeState = GaugeState1.Fill;
            UseGauge(currentGaugeState); // 게이지 소모 완료 후 초기화 상태로 복귀
            consumeSequence = null;
        });

        // 시퀀스 시작 (자동 재생)
        return sequence;
    }

    #region 해제 파트
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ReleaseGaugeHandle(); // 모든 게이지 핸들 해제
    }

    // 모든 로드 핸들 해제 (게이지 아예 제거할 때 호출)
    public void ReleaseGaugeHandle()
    {
        foreach (var comp in fillGauges)
        {
            // 각 컴포넌트가 자신이 관리하는 인스턴스를 해제하도록 요청
            comp.ReleaseInstance();
        }
        fillGauges.Clear(); // 관리 리스트 비우기
    }
    #endregion 해제 파트
}