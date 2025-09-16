using System;
using System.Collections.Generic;

// 아이템 리스트를 갖고 있고, 아이템을 관리할 수 있는 Model 계층의 클래스 (빼고 넣기 등)
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