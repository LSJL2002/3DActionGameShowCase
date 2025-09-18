using System;
using System.Collections.Generic;
using UnityEngine;

// Inventory(Model)의 데이터를 받아서 InventoryUI(View)가 쉽게 표시할 수 있도록 가공하고,
// InventoryUI(View)의 사용자 입력을 받아서 Inventory(Model) 모델에 전달
public class InventoryViewModel
{
    // (구독:인벤토리UI(View))
    public event Action OnConsumableUIUpdate;
    public event Action OnSkillUIUpdate;
    public event Action OnCoreUIUpdate;

    private Inventory _model;

    public InventoryViewModel(Inventory model)
    {
        _model = model;
        // 모델의 각 이벤트에 뷰모델의 메서드를 구독
        _model.OnConsumableItemsChanged += OnConsumableItemsChanged;
        _model.OnSkillItemsChanged += OnSkillItemsChanged;
        _model.OnCoreItemsChanged += OnCoreItemsChanged;
    }

    private void OnConsumableItemsChanged()
    {
        OnConsumableUIUpdate?.Invoke();
    }

    private void OnSkillItemsChanged()
    {
        OnSkillUIUpdate?.Invoke();
    }

    private void OnCoreItemsChanged()
    {
        OnCoreUIUpdate?.Invoke();
    }

    // 각 탭의 뷰가 호출하여 데이터를 가져갈 메서드
    public IReadOnlyList<InventoryItem> GetConsumableItems()
    {
        return _model.GetConsumableItems();
    }

    public IReadOnlyList<InventoryItem> GetSkillItems()
    {
        return _model.GetSkillItems();
    }

    public IReadOnlyList<InventoryItem> GetCoreItems()
    {
        return _model.GetCoreItems();
    }
}