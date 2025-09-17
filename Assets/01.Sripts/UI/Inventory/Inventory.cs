using System;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

// 인벤토리의 아이템'들'에 대한 정보(리스트)를 가진 클래스 (Model 계층)
// 리스트 중 특정 아이템을 찾을 수 있도록 도와줌
public class Inventory
{
    public event Action OnConsumableItemsChanged;
    public event Action OnSkillItemsChanged;
    public event Action OnCoreItemsChanged;

    private List<InventoryItem> consumableItems = new List<InventoryItem>();
    private List<InventoryItem> skillItems = new List<InventoryItem>();
    private List<InventoryItem> coreItems = new List<InventoryItem>();

    // 아이템을 추가하는 메인 메서드
    public void AddItem(ItemData itemData, int count)
    {
        switch (itemData.itemType)
        {
            case ItemType.Consumable:
                AddItemToCollection(consumableItems, itemData, count);
                OnConsumableItemsChanged?.Invoke();
                break;
            case ItemType.Skill:
                AddItemToCollection(skillItems, itemData, count);
                OnSkillItemsChanged?.Invoke();
                break;
            case ItemType.Core:
                AddItemToCollection(coreItems, itemData, count);
                OnCoreItemsChanged?.Invoke();
                break;
        }
    }

    private void AddItemToCollection(List<InventoryItem> collection, ItemData itemData, int count)
    {
        // 이미 존재하는 아이템 찾기
        InventoryItem existingItem = collection.Find(item => item.data == itemData);

        // 아이템이 있다면 개수 업데이트, 없다면 추가
        if (existingItem != null)
        {
            existingItem.stackCount += count;
        }
        else
        {
            collection.Add(new InventoryItem(itemData, count));
        }
    }

    // 각 카테고리의 아이템 리스트를 반환하는 메서드
    public IReadOnlyList<InventoryItem> GetConsumableItems() => consumableItems;
    public IReadOnlyList<InventoryItem> GetSkillItems() => skillItems;
    public IReadOnlyList<InventoryItem> GetCoreItems() => coreItems;
}