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
    [SerializeField] private List<GaugeComponent> activeGauges = new List<GaugeComponent>();
    [SerializeField] private string gaugePrefabAddress = "Gauge_Mask";

    protected async override void Start()
    {
        base.Start();
        
        // FillGauge함수는 정상 작동하는 것을 확인했음. 공격시 게이지가 안되는것 같음
        UIManager.Instance.playerInput.Player.HeavyAttack.performed += FillGauge;

        for (int i = 0; i < 15; i++)
        {
            await AddGaugeAsync(); // 초기 게이지 15개 추가
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        UIManager.Instance.playerInput.Player.HeavyAttack.performed -= FillGauge;
        
        ReleaseGaugeHandle(); // 모든 게이지 핸들 해제
    }

    // 게이지핸들 로드 함수 (어드레서블 로드 및 List 관리 방식)
    // UniTask : 이 함수가 비동기 함수임을 나타내는 반환 타입
    // GaugeComponent : 이 함수가 성공적으로 완료되었을 때 반환하는 값의 타입
    public async UniTask<GaugeComponent> AddGaugeAsync()
    {
        var newHandle = Addressables.InstantiateAsync(gaugePrefabAddress, gaugeContainer.transform);
        await newHandle;

        // 성공 시 처리
        if (newHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GaugeComponent gaugeComp = newHandle.Result.GetComponent<GaugeComponent>();
            gaugeComp.SetHandle(newHandle); // 생성된 핸들을 설정
            activeGauges.Add(gaugeComp); // 리스트에 추가
            return gaugeComp; // GaugeComponent 객체를 반환            
        }
        return null;
    }

    // 게이지핸들 1개씩 FillImage 활성화 (게이지 채울 때 호출)
    public void FillGauge(InputAction.CallbackContext context)
    {
        foreach (var comp in activeGauges)
        {
            if (comp.isActiveFillImage == false)
            {
                comp.SetFill();
                break; // 하나만 활성화 후 종료
            }
        }
    }

    // 게이지 애니메이션 시작 (게이지 해방시 호출)
    public void StartGaugeAnimation()
    {
        for (int i = 0; i < activeGauges.Count; i++)
        {
            activeGauges[i].StartWaveAnimation(i * 0.03f);
        }
    }

    // 모든 로드 핸들 비활성화 (게이지 해방 후 초기화 시 호출)
    public void ResetGaugeHandle()
    {
        foreach (var comp in activeGauges)
        {
            comp.gameObject.SetActive(false);
        }
    }

    // 모든 로드 핸들 해제 (게이지 아예 제거할 때 호출)
    public void ReleaseGaugeHandle()
    {
        foreach (var comp in activeGauges)
        {
            // 각 컴포넌트가 자신이 관리하는 인스턴스를 해제하도록 요청
            comp.ReleaseInstance();
        }
        activeGauges.Clear(); // 관리 리스트 비우기
    }
}