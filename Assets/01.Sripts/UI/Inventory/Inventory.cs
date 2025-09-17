using System;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리의 아이템'들'에 대한 정보(리스트)를 가진 클래스 (Model 계층)
// 리스트 중 특정 아이템을 찾을 수 있도록 도와줌
public class Inventory
{
    public event Action OnInventoryChanged; // 인벤토리 변경 알림 (구독:인벤토리뷰모델)

    private List<InventoryItem> _items = new List<InventoryItem>();
    public IReadOnlyList<InventoryItem> Items => _items;

    public void AddItem(ItemData itemData, int count)
    {
        // 동일한 아이템이 있는지 찾아 스택에 추가
        InventoryItem existingItem = _items.Find(item => item.data == itemData);

        if (existingItem != null)
        {
            existingItem.stackCount += count;
        }
        else
        {
            _items.Add(new InventoryItem(itemData, count));
        }

        OnInventoryChanged?.Invoke();
    }
}