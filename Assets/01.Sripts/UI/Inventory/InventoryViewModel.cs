using System;
using System.Collections.Generic;

// Inventory(Model)의 데이터를 받아서 InventoryUI(View)가 쉽게 표시할 수 있도록 가공하고,
// InventoryUI(View)의 사용자 입력을 받아서 Inventory(Model) 모델에 전달
public class InventoryViewModel
{
    private Inventory _model;
    public event Action OnUpdateUI; // (구독:인벤토리뷰(인벤토리UI))

    public InventoryViewModel(Inventory model)
    {
        _model = model;
        _model.OnInventoryChanged += OnModelChanged;
    }

    private void OnModelChanged()
    {
        OnUpdateUI?.Invoke();
    }

    public IReadOnlyList<InventoryItem> GetItems()
    {
        return _model.Items;
    }
}