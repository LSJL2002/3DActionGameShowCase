using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class AwakenUIElement
{
    public GameObject gaugeContainer;
    public CanvasGroup canvasGroup;
    public List<GaugeComponent> fillGauges = new List<GaugeComponent>();
}

public class AwakenUI : UIBase, IInterfaceOpen
{
    public enum GaugeState1
    {
        Fill, // 충전중
        Full, // 만땅
        Awaken, // 사용중
    }

    private GaugeState1 currentGaugeState = GaugeState1.Fill;

    [Header("[Awaken UI Group]")]
    [SerializeField] private AwakenUIElement[] awakenUIElements = new AwakenUIElement[3];
    [SerializeField] CanvasGroup awakenGaugeCanvasGroup;
    [SerializeField] private string gaugePrefabAddress = "Gauge_Mask";

    private int playerIndex;
    private float gaugeCount = 50;

    protected override void OnEnable()
    {
        InstanceFillGauge(); // 게이지 종류별로 FillGauge 인스턴스 (gaugeCount만큼)
        
        // 구독
        PlayerManager.Instance.Attr.AwakenGauge.OnChanged += UpdateGauge;
        PlayerManager.Instance.Attr.AwakenGauge.OnFull += ChangeFullState;
        PlayerManager.Instance.Attr.AwakenGauge.OnUsed += ChangeAwakenState;
        PlayerManager.Instance.OnActiveCharacterChanged += UpdateReference;

        PlayerCharacter activeCharacter = PlayerManager.Instance.ActiveCharacter;
        UpdateReference(activeCharacter);
        
        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { awakenGaugeCanvasGroup.DOFade(1f, 1f); });

        EventListen();
    }

    // 최초, 플레이어 교체시 호출하여 각성게이지 참조들을 갱신
    public void UpdateReference(PlayerCharacter playerCharacter)
    {
        playerIndex = (int)playerCharacter.CharacterType; // 현재 플레이어 번호 갱신
        for (int i = 0; i < awakenUIElements.Length; i++)
        {
            if (playerIndex == i && PlayerManager.Instance.Attr.AwakenGauge != null) // 플레이어 게이지정보가 null이 아니라면,
            {
                awakenUIElements[i].canvasGroup.alpha = 1f;
            }
            else awakenUIElements[i].canvasGroup.alpha = 0f;
        }
    }

    // 초기에 게이지컨테이너 하위에 Fill 게이지 컴포넌트 전체 추가하는 함수
    public async void InstanceFillGauge()
    {
        // AwakenUIElement 개수만큼 순회
        for (int i = 0; i < awakenUIElements.Length; i++)
        {
            // 최대 게이지 개수만큼 Gauge Component 생성 및 List에 추가
            for (int j = 0; j < gaugeCount; j++)
            {
                await AddGaugeAsync(i);
            }
            StartGaugeAnimation(i); // 모든 게이지 애니메이션 시작 (알파값 0상태라 안보임)
        }
    }

    // 게이지핸들 로드 함수 (어드레서블 로드 및 List 관리 방식)
    // UniTask : 이 함수가 비동기 함수임을 나타내는 반환 타입
    // GaugeComponent : 이 함수가 성공적으로 완료되었을 때 반환하는 값의 타입(Class명)
    public async UniTask<GaugeComponent> AddGaugeAsync(int i)
    {
        var newHandle = Addressables.InstantiateAsync(gaugePrefabAddress, awakenUIElements[i].gaugeContainer.transform);
        await newHandle;
        // 성공 시 처리
        if (newHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GaugeComponent gaugeComp = newHandle.Result.GetComponent<GaugeComponent>();
            gaugeComp.SetHandle(newHandle); // 생성된 핸들을 설정
            awakenUIElements[i].fillGauges.Add(gaugeComp); // 리스트에 추가
            return gaugeComp; // GaugeComponent 객체를 반환
        }
        return null;
    }

    // 웨이브 연출 시작 함수
    public void StartGaugeAnimation(int i)
    {
        for (int j = 0; j < awakenUIElements[playerIndex].fillGauges.Count; j++)
        {
            awakenUIElements[playerIndex].fillGauges[j].StartWaveEffect(j * 0.1f);
        }
    }

    // 현재 게이지값에 맞게 게이지 상태 업데이트하는 함수
    public void UpdateGauge(float amount)
    {
        if (currentGaugeState == GaugeState1.Full) return; // 현재 Full 상태라면 리턴
        int countToActivate = Mathf.FloorToInt(amount);

        for (int i = 0; i < awakenUIElements[playerIndex].fillGauges.Count; i++)
        {
            if (i < countToActivate)
            {
                switch (currentGaugeState) // 켜져야하는 게이지들은 현재상태에 따라 켜줌 (On/Awaken)
                {
                    case GaugeState1.Fill:
                        awakenUIElements[playerIndex].fillGauges[i].SetGauge(GaugeComponent.GaugeState2.On);
                        break;
                    case GaugeState1.Awaken:
                        awakenUIElements[playerIndex].fillGauges[i].SetGauge(GaugeComponent.GaugeState2.Awaken);
                        break;
                }
            }
            else // 켜지면 안되는 것들은 꺼줌
            {
                // 현재 Off가 아니라면 Off가 맞는 것들이니, 꺼줌
                if (awakenUIElements[playerIndex].fillGauges[i].currentGaugeState != GaugeComponent.GaugeState2.Off)
                {
                    awakenUIElements[playerIndex].fillGauges[i].SetGauge(GaugeComponent.GaugeState2.Off);
                }
            }
        }
        if (amount <= 0) { ChangeGaugeState(GaugeState1.Fill); } // 변경된 값이 0이하가 되면 Fill 상태로 변경
    }

    public void ChangeFullState() { ChangeGaugeState(GaugeState1.Full); }
    public void ChangeAwakenState() { ChangeGaugeState(GaugeState1.Awaken); }
    public void ChangeGaugeState(GaugeState1 gaugeState1)
    {
        currentGaugeState = gaugeState1;
        Debug.Log($"currentGaugeState:{currentGaugeState}");
        var currentElements = awakenUIElements[playerIndex].fillGauges;

        switch (gaugeState1)
        {
            case GaugeState1.Fill:
                foreach (var comp in currentElements)
                {
                    comp.SetGauge(GaugeComponent.GaugeState2.Off);
                }
                break;

            case GaugeState1.Full:
                foreach (var comp in awakenUIElements[playerIndex].fillGauges)
                {
                    comp.PopGaugeEffect(); // fillImage 알파값을 n으로 변경
                }
                break;

            case GaugeState1.Awaken:
                foreach (var comp in currentElements)
                {
                    comp.SetGauge(GaugeComponent.GaugeState2.Awaken);
                }
                break;
        }
    }

    public void EventListen()
    {
        EventsManager.Instance.StartListening(GameEvent.OnMenu, Interact);
    }

    public void Interact()
    {
        awakenGaugeCanvasGroup.alpha = (awakenGaugeCanvasGroup.alpha == 0f) ? 1f : 0f;
    }

    #region 해제 파트
    protected override void OnDisable()
    {
        base.OnDisable();

        if (PlayerManager.Instance != null)
        {
            // 구독해제
            PlayerManager.Instance.Attr.AwakenGauge.OnChanged -= UpdateGauge;
            PlayerManager.Instance.Attr.AwakenGauge.OnFull -= ChangeFullState;
            PlayerManager.Instance.Attr.AwakenGauge.OnUsed -= ChangeAwakenState;
            PlayerManager.Instance.OnActiveCharacterChanged -= UpdateReference;
        }

        if (EventsManager.Instance != null) EventsManager.Instance.StopListening(GameEvent.OnMenu, Interact);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ReleaseGaugeHandle(); // 모든 게이지 핸들 해제
    }

    // 모든 로드 핸들 해제 (게이지 아예 제거할 때 호출)
    public void ReleaseGaugeHandle()
    {
        foreach (var comp in awakenUIElements[playerIndex].fillGauges)
        {
            // 각 컴포넌트가 자신이 관리하는 인스턴스를 해제하도록 요청
            comp.ReleaseInstance();
        }
        awakenUIElements[playerIndex].fillGauges.Clear(); // 관리 리스트 비우기
    }
    #endregion 해제 파트
}