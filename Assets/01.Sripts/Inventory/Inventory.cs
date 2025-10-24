using System.Collections.Generic;
using System.Linq;
using static ItemData;

public class Inventory
{
    private Dictionary<ItemData, InventoryItem> consumableItems = new Dictionary<ItemData, InventoryItem>();
    private Dictionary<ItemData, InventoryItem> skillItems = new Dictionary<ItemData, InventoryItem>();
    private Dictionary<ItemData, InventoryItem> coreItems = new Dictionary<ItemData, InventoryItem>();

    // 아이템 추가 함수 (인벤토리 매니저에서 호출)
    public void AddItem(ItemData itemData, int count)
    {
        switch (itemData.itemType)
        {
            case ItemType.Consumable:
                AddItemToCollection(consumableItems, itemData, count);
                EventsManager.Instance.TriggerEvent(GameEvent.OnConsumableItemsChanged);
                break;
            case ItemType.SkillCard:
                AddItemToCollection(skillItems, itemData, count);
                EventsManager.Instance.TriggerEvent(GameEvent.OnSkillItemsChanged);
                break;
            case ItemType.Core:
                AddItemToCollection(coreItems, itemData, count);
                EventsManager.Instance.TriggerEvent(GameEvent.OnCoreItemsChanged);
                break;
        }
    }

    // 딕셔너리에 아이템 추가 함수
    private void AddItemToCollection(Dictionary<ItemData, InventoryItem> collection, ItemData itemData, int count)
    {
        // 딕셔너리에서 아이템을 키로 사용하여 존재 여부 확인
        if (collection.ContainsKey(itemData))
        {
            collection[itemData].stackCount += count;
        }
        else
        {
            // 새로운 InventoryItem 객체 추가
            collection.Add(itemData, new InventoryItem(itemData, count));
        }
    }

    // 아이템 수량 감소 함수 (인벤토리 매니저에서 호출)
    public void DecreaseItemCount(ItemData itemData, int count)
    {
        switch (itemData.itemType)
        {
            case ItemType.Consumable:
                DecreaseItemFromCollection(consumableItems, itemData, count);
                EventsManager.Instance.TriggerEvent(GameEvent.OnConsumableItemsChanged);
                break;
        }
    }

    // 딕셔너리에서 아이템 수량 감소 및 삭제
    private void DecreaseItemFromCollection(Dictionary<ItemData, InventoryItem> collection, ItemData itemData, int count)
    {
        if (collection.ContainsKey(itemData))
        {
            collection[itemData].stackCount -= count;

            if (collection[itemData].stackCount <= 0)
            {
                collection.Remove(itemData);
            }
        }
    }

    // 모든 딕셔너리 클리어 함수 (모든 스테이지 클리어시 정리용)
    public void ClearDictionary()
    {
        consumableItems = new Dictionary<ItemData, InventoryItem>();
        skillItems = new Dictionary<ItemData, InventoryItem>();
        coreItems = new Dictionary<ItemData, InventoryItem>();
    }

    // 반환하는 메서드도 Dictionary의 Values를 List로 변환하여 반환
    public IReadOnlyList<InventoryItem> GetConsumableItems() => consumableItems.Values.ToList();
    public IReadOnlyList<InventoryItem> GetSkillItems() => skillItems.Values.ToList();
    public IReadOnlyList<InventoryItem> GetCoreItems() => coreItems.Values.ToList();
}