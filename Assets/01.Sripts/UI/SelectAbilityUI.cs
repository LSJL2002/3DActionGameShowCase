using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SelectAbilityUI : UIBase
{
    [SerializeField] private CanvasGroup canvasGroup;

    // 배틀매니저가 가지고 있는 아이템을 저장할 변수
    private string statUPItemAdress;
    private string consumableItemAdress1;
    private string consumableItemAdress2;
    private string skillItemAdress;
    private string coreItemAdress;

    // AsyncOperationHandle을 저장할 변수 추가
    private AsyncOperationHandle<ItemData> statUPLoadHandle;
    private AsyncOperationHandle<ItemData> consumableLoadHandle1;
    private AsyncOperationHandle<ItemData> consumableLoadHandle2;
    private AsyncOperationHandle<ItemData> skillLoadHandle;
    private AsyncOperationHandle<ItemData> coreLoadHandle;

    [SerializeField] private List<ItemSlotUI> itemSlots;

    protected override void OnEnable()
    {
        base.OnEnable();

        // 플레이어 입력 제어
        PlayerManager.Instance.EnableInput(false);

        canvasGroup.DOFade(0f, 0f).OnComplete(() => { canvasGroup.DOFade(1f, 1.5f); });

        UIManager.Instance.ChangeState(DecisionState.SelectAbility);

        AudioManager.Instance.PlaySFX("OpenSelectAbilityUISound");

        BattleManager battleManager = BattleManager.Instance;

        // 변수 초기화
        statUPItemAdress = battleManager.GetItemInfo(0); // n번째정보 : 스탯업ID
        consumableItemAdress1 = battleManager.GetItemInfo(1); // n번째정보 : 스킬아이템ID
        consumableItemAdress2 = battleManager.GetItemInfo(2); // n번째정보 : 코어아이템ID
        //skillItemAdress = battleManager.GetItemInfo(1); // n번째정보 : 스킬아이템ID
        //coreItemAdress = battleManager.GetItemInfo(2); // n번째정보 : 코어아이템ID

        GetItemInfo(statUPItemAdress, "StatUP"); // 스탯 아이템 정보 가져오고 UI에 세팅
        GetItemInfo(consumableItemAdress1, "Consumable1"); // 소비 아이템1 정보 가져오고 UI에 세팅
        GetItemInfo(consumableItemAdress2, "Consumable2"); // 소비 아이템2 정보 가져오고 UI에 세팅
        //GetItemInfo(skillItemAdress, "Skill"); // 스킬 아이템 정보 가져오고 UI에 세팅
        //GetItemInfo(coreItemAdress, "Core");   // 코어 아이템 정보 가져오고 UI에 세팅
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // 로드했던 데이터 해제
        ReleaseHandles();

        // 플레이어 입력 제어 해제
        PlayerManager.Instance.EnableInput(false);

       
    }

    public async void GetItemInfo(string adress, string type)
    {
        // 타입 확인 / 이미 핸들이 유효한지 확인
        if (type == "StatUP" && statUPLoadHandle.IsValid())
        {
            Addressables.Release(statUPLoadHandle);
        }
        if (type == "Consumable1" && consumableLoadHandle1.IsValid())
        {
            Addressables.Release(consumableLoadHandle1);
        }
        if (type == "Consumable2" && consumableLoadHandle2.IsValid())
        {
            Addressables.Release(consumableLoadHandle2);
        }
        if (type == "Skill" && skillLoadHandle.IsValid())
        {
            Addressables.Release(skillLoadHandle);
        }
        else if (type == "Core" && coreLoadHandle.IsValid())
        {
            Addressables.Release(coreLoadHandle);
        }

        // 어드레서블로 아이템 데이터 로드
        AsyncOperationHandle<ItemData> loadHandle = Addressables.LoadAssetAsync<ItemData>(adress);

        if (type == "StatUP")
        {
            statUPLoadHandle = loadHandle;
        }
        if (type == "Consumable1")
        {
            consumableLoadHandle1 = loadHandle;
        }
        if (type == "Consumable2")
        {
            consumableLoadHandle2 = loadHandle;
        }
        if (type == "Skill")
        {
            skillLoadHandle = loadHandle;
        }
        else if (type == "Core")
        {
            coreLoadHandle = loadHandle;
        }

        // 로딩 완료까지 대기
        await loadHandle.Task;

        // 로딩이 완료된 시점에 핸들 유효성 확인
        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ItemData loadedItem = loadHandle.Result;

            if (type == "StatUP")
            {
                // 아이템슬롯 리스트의 [n]번째 슬롯에 불러온 아이템을 할당
                itemSlots[0].SetData(loadedItem);
            }
            if (type == "Consumable1")
            {
                // 아이템슬롯 리스트의 [n]번째 슬롯에 불러온 아이템을 할당
                itemSlots[1].SetData(loadedItem);
            }
            if (type == "Consumable2")
            {
                // 아이템슬롯 리스트의 [n]번째 슬롯에 불러온 아이템을 할당
                itemSlots[2].SetData(loadedItem);
            }
            if (type == "Skill")
            {
                // 아이템슬롯 리스트의 [n]번째 슬롯에 불러온 아이템을 할당
                itemSlots[1].SetData(loadedItem);
            }
            else if (type == "Core")
            {
                // 아이템슬롯 리스트의 [n]번째 슬롯에 불러온 아이템을 할당
                itemSlots[2].SetData(loadedItem);
            }
        }
        else
        {
            Debug.LogError($"{adress} Fail");
        }
    }

    // 모든 핸들을 해제하는 함수
    private void ReleaseHandles()
    {
        if (statUPLoadHandle.IsValid())
        {
            Addressables.Release(statUPLoadHandle);
            Debug.Log("스탯업 데이터 해제 완료");
        }
        if (consumableLoadHandle1.IsValid())
        {
            Addressables.Release(consumableLoadHandle1);
            Debug.Log("스킬 데이터 해제 완료");
        }
        if (consumableLoadHandle2.IsValid())
        {
            Addressables.Release(consumableLoadHandle2);
            Debug.Log("스킬 데이터 해제 완료");
        }
        if (skillLoadHandle.IsValid())
        {
            Addressables.Release(skillLoadHandle);
            Debug.Log("스킬 데이터 해제 완료");
        }
        if (coreLoadHandle.IsValid())
        {
            Addressables.Release(coreLoadHandle);
            Debug.Log("코어 데이터 해제 완료");
        }
    }
}
