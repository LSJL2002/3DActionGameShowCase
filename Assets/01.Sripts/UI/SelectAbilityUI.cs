using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class SelectAbilityUI : UIBase
{
    [SerializeField] private CanvasGroup canvasGroup;

    // 배틀매니저가 가지고 있는 아이템을 저장할 변수
    private string statUPItemAdress;
    private string skillItemAdress;
    private string coreItemAdress;

    // AsyncOperationHandle을 저장할 변수 추가
    private AsyncOperationHandle<ItemData> statUPLoadHandle;
    private AsyncOperationHandle<ItemData> skillLoadHandle;
    private AsyncOperationHandle<ItemData> coreLoadHandle;

    private ItemData skillHandle; // 테스트
    private ItemData coreHandle; // 테스트

    [SerializeField] private List<ItemSlotUI> itemSlots;

    // (구독:인벤토리매니저)
    public static event Action OnSelectAbilityUI;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        canvasGroup.DOFade(0f, 0f).OnComplete(() => { canvasGroup.DOFade(1f, 2f); });

        OnSelectAbilityUI?.Invoke();

        // 마우스 커서 보이게
        PlayerManager.Instance.EnableInput(false);

        BattleManager battleManager = BattleManager.Instance;

        // 변수 초기화
        statUPItemAdress = battleManager.GetItemInfo(0); // n번째정보 : 스탯업ID
        skillItemAdress = battleManager.GetItemInfo(1); // n번째정보 : 스킬아이템ID
        coreItemAdress = battleManager.GetItemInfo(2); // n번째정보 : 코어아이템ID

        GetItemInfo(statUPItemAdress, "StatUP"); // 스킬 아이템 정보 가져오고 UI에 세팅
        GetItemInfo(skillItemAdress, "Skill"); // 스킬 아이템 정보 가져오고 UI에 세팅
        GetItemInfo(coreItemAdress, "Core");   // 코어 아이템 정보 가져오고 UI에 세팅
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // 로드했던 데이터 해제
        ReleaseHandles();

        BattleManager.Instance.ClearBattle();

        // 마우스 커서 다시 락
        PlayerManager.Instance.EnableInput(true);
    }

    public async void GetItemInfo(string adress, string type)
    {
        // 타입 확인 / 이미 핸들이 유효한지 확인
        if (type == "StatUP" && statUPLoadHandle.IsValid())
        {
            Addressables.Release(statUPLoadHandle);
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

    public void OnClickButton(string str)
    {
        AudioManager.Instance.PlaySFX("ButtonSoundEffect");
        DOVirtual.DelayedCall(0.2f, () => { }); // 아무것도 없이 n초간 대기

        InventoryManager.Instance.LoadData_Addressables("20000000");
        Debug.Log("회복약 획득");

        switch (str)
        {
            case "Stat":
                // 플레이어 스탯 증가 함수 호출
                PlayerManager.Instance.Stats.AddModifier(StatType.MaxHealth, itemSlots[0].itemData.MaxHP);
                PlayerManager.Instance.Stats.AddModifier(StatType.MaxEnergy, itemSlots[0].itemData.MaxMP);
                PlayerManager.Instance.Stats.AddModifier(StatType.Attack, itemSlots[0].itemData.Attack);
                PlayerManager.Instance.Stats.AddModifier(StatType.Defense, itemSlots[0].itemData.Defence);
                PlayerManager.Instance.Stats.AddModifier(StatType.MoveSpeed, itemSlots[0].itemData.MoveSpeed);
                PlayerManager.Instance.Stats.AddModifier(StatType.AttackSpeed, itemSlots[0].itemData.AttackSpeed);

                Debug.Log($"플레이어 스탯증가");
                break;
        }
    }
}
