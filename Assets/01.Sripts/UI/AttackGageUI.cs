using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AttackGageUI : UIBase
{
    [SerializeField] private GameObject gaugeContainer; // 프리펩을 추가할 부모 오브젝트
    [SerializeField] private List<AsyncOperationHandle<GameObject>> gaugeList = new List<AsyncOperationHandle<GameObject>>(); // 추가된 프리펩을 저장할 리스트
    [SerializeField] private string gaugePrefabAddress = "Gauge_Mask"; // 프리펩 주소

    // 프리펩을 추가하는 메서드 (공격시 호출)
    public void AddGauge()
    {
        var newHandle = Addressables.InstantiateAsync(gaugePrefabAddress, gaugeContainer.transform);
        gaugeList.Add(newHandle);
    }

    // 리스트를 비우는 메서드 (게이지 해방 후 호출)
    public void ClearGauges()
    {
        foreach (var handle in gaugeList)
        {
            if (handle.IsValid())
            {
                Addressables.ReleaseInstance(handle);
            }
        }
        gaugeList.Clear();
    }

    // Gauge 물결 애니메이션 시작
    public void StartGaugeAnimation()
    {
        foreach (var handle in gaugeList)
        {
            if (handle.IsValid())
            {
                handle.Result.GetComponent<Transform>().DOMoveY(10f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    private void Start()
    {
        StartGaugeAnimation();
    }
}
