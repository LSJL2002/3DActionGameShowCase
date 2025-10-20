using System.Collections.Generic;
using UnityEngine;

// 아이템 슬롯부분을 제외한 인벤토리 전체의 UI를 보여주는 View 계층의 클래스
// 아이템에 직접 접근할 수는 없고 ViewModel과 이벤트를 통해 아이템이 변경 될 수 있도록 신호를 보내는 역할
// CharacterInfomationUI의 Inventory Part
public partial class CharacterInfomationUI : UIBase
{
    // 인스펙터에서 직접 할당할 아이템 슬롯 목록
    [SerializeField] private List<ItemSlotUI> itemSlots;
    [SerializeField] private InventoryViewModel inventoryViewModel;

    public void OnEnableInventory()
    {
        UIManager.Instance.ChangeState(DecisionState.UseItem);
    }

    public void Setup()
    {
        EventsManager.Instance.StartListening(GameEvent.OnConsumableUIUpdate, UpdateUI);
        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(EventsManager.Instance != null)
            EventsManager.Instance.StopListening(GameEvent.OnConsumableUIUpdate, UpdateUI);
    }

    private void UpdateUI()
    {
        var items = inventoryViewModel.GetConsumableItems();
        int slotCount = itemSlots.Count;
        int itemCount = items.Count;

        for (int i = 0; i < slotCount; i++)
        {
            if (i < itemCount)
            {
                // 아이템이 있는 경우 데이터 할당
                itemSlots[i].SetData(items[i].data, items[i].stackCount);
            }
            else
            {
                // 아이템이 없는 경우 슬롯을 초기화 (내용만 지움)
                itemSlots[i].ClearSlot();
            }
        }
    }
}
