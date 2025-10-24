using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InventoryManager : Singleton<InventoryManager>
{
    private Inventory inventoryModel;
    [SerializeField] private InventoryViewModel inventoryViewModel;

    protected override void Start()
    {
        base.Start();

        inventoryModel = new Inventory();
        inventoryViewModel.Init(inventoryModel);
    }

    // 아이템 추가 함수
    public async void LoadData_Addressables(string adress, int stack = default)
    {
        // 어드레서블로 아이템 데이터 로드
        AsyncOperationHandle<ItemData> loadHandle = Addressables.LoadAssetAsync<ItemData>(adress);

        // 로딩 완료까지 대기
        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ItemData loadedItem = loadHandle.Result;
            inventoryModel.AddItem(loadedItem, stack);
        }
        else
        {
            Debug.LogError($"{adress} Fail");
        }
    }

    // 아이템 감소 및 삭제 함수
    public void UseConsumableItem(ItemData itemData)
    {
        // 아이템의 효과리스트를 전부 발동
        for (int i = 0; i < itemData.abilities.Count; i++ )
        {
            itemData.abilities[i].Use(itemData);
        }

        // Model에게 아이템 수량 감소를 요청
        inventoryModel.DecreaseItemCount(itemData, 1);

        EventsManager.Instance.TriggerEvent(GameEvent.OnUsedItem);
    }

    // 인벤토리 소비아이템 리스트를 가져오는 함수 (읽기전용)
    public IReadOnlyList<InventoryItem> GetConsumableItems()
    {
        return inventoryViewModel.GetConsumableItems();
    }

    // 인벤토리 스킬카드아이템 리스트를 가져오는 함수 (읽기전용)
    public IReadOnlyList<InventoryItem> GetSkillItems()
    {
        return inventoryViewModel.GetSkillItems();
    }

    // 인벤토리 코어아이템 리스트를 가져오는 함수 (읽기전용)
    public IReadOnlyList<InventoryItem> GetCoreItems()
    {
        return inventoryViewModel.GetCoreItems();
    }

    // 스테이지 전부 클리어시 인벤토리 비우는 함수
    public void ClearAllInventory()
    {
        inventoryModel.ClearDictionary();
    }

    // 플레이어 스탯 추가 함수 (능력선택시 호출)
    public void StatUPAbility(ItemData itemData)
    {
        // 플레이어 스탯 증가 함수 호출
        PlayerManager.Instance.Attr.AddModifier(StatType.MaxHealth, itemData.MaxHP);
        PlayerManager.Instance.Attr.AddModifier(StatType.MaxEnergy, itemData.MaxMP);
        PlayerManager.Instance.Attr.AddModifier(StatType.Attack, itemData.Attack);
        PlayerManager.Instance.Attr.AddModifier(StatType.Defense, itemData.Defence);
        PlayerManager.Instance.Attr.AddModifier(StatType.MoveSpeed, itemData.MoveSpeed);
        PlayerManager.Instance.Attr.AddModifier(StatType.AttackSpeed, itemData.AttackSpeed);

        EventsManager.Instance.TriggerEvent(GameEvent.OnStatChanged);
    }
}