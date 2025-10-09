using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AttackGaugeUI : UIBase
{
    [SerializeField] private GameObject gaugeContainer;
    [SerializeField] private List<GaugeComponent> fillGauges = new List<GaugeComponent>();
    [SerializeField] private string gaugePrefabAddress = "Gauge_Mask";

    private bool allGaugesFull = false;

    protected async override void Awake()
    {
        base.Awake();

        // 초기에 최대 게이지 개수 추가(빈칸 상태)
        int maxGaugeCount = 50;
        for (int i = 0; i < maxGaugeCount; i++)
        {
            await AddGaugeAsync();
        }

        // 모든 게이지 애니메이션 시작 (알파값 0상태라 안보임)
        StartGaugeAnimation();
    }

    protected override void Start()
    {
        base.Start();
        
        PlayerManager.Instance.Input.PlayerActions.Attack.started += UpdateGauge; // 일반공격 입력시 구독
        PlayerManager.Instance.Input.PlayerActions.HeavyAttack.started += UpdateGauge; // 스킬공격 입력시 구독
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        PlayerManager.Instance.Input.PlayerActions.Attack.started -= UpdateGauge; // 일반공격 입력시 구독 해제
        PlayerManager.Instance.Input.PlayerActions.HeavyAttack.started -= UpdateGauge; // 스킬공격 입력시 구독 해제

        ReleaseGaugeHandle(); // 모든 게이지 핸들 해제
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

    // 게이지 업데이트 함수 (추가/감소 함께 사용)
    public void UpdateGauge(InputAction.CallbackContext context)
    {
        if (allGaugesFull) return; // 이미 모두 찼다면 무시

        // 플레이어쪽 현재스택을 읽어와서 게이지를 업데이트

        foreach (var comp in fillGauges)
        {
            if (comp.isFillImage == false)
            {
                comp.SetFillGauge();
                break; // 하나만 적용 후 종료
            }
        }

        // 모든 게이지가 찼다면
        if (!allGaugesFull && fillGauges.TrueForAll(g => g.isFillImage))
        {
            allGaugesFull = true; // 상태 업데이트

            foreach (var comp in fillGauges)
            {
                // fillImage 알파값을 n으로 변경
                comp.PopGaugeEffect();
            }
        }
    }

    // 게이지사용시 컬러 세팅 함수 (최초 한번만 호출해서 색바꾸고 상태 업데이트)
    public void UseGauge()
    {
        if (!allGaugesFull) return; // 모두 찼을 때만 사용 가능

        allGaugesFull = false; // 상태 업데이트

        // 전체 fillImage 컬러를 변경
        foreach (var comp in fillGauges)
        {
            // fillImage 알파값을 0으로 변경
            comp.SetColor(GaugeComponent.GaugeState.Use);
        }
    }

    // 모든 로드 핸들 비활성화 (게이지 해방 후 초기화 시 호출)
    public void ResetGaugeHandle()
    {
        foreach (var comp in fillGauges)
        {
            comp.gameObject.SetActive(false);
        }
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
}