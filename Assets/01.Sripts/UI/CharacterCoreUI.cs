using System.Collections.Generic;
using UnityEngine;

public class CharacterCoreUI : UIBase
{
    // 인스펙터에서 직접 할당할 아이템 슬롯 목록
    [SerializeField] private List<ItemSlotUI> itemSlots;
    [SerializeField] private InventoryViewModel inventoryViewModel;

    protected override void Awake()
    {
        base.Awake();
        Setup();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UIManager.Instance.ChangeState(DecisionState.UseItem);
    }

    public void Setup()
    {
        EventsManager.Instance.StartListening(GameEvent.OnCoreUIUpdate, UpdateUI);
        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventsManager.Instance.StopListening(GameEvent.OnCoreUIUpdate, UpdateUI);
    }

    private void UpdateUI()
    {
        var items = inventoryViewModel.GetCoreItems();
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

    // 스킬장착 함수 (스킬카드 버튼에서 호출)
    // 이미 장착상태일시 해제되도록 로직 구성
    public void SetSkill(string str)
    {
        
    }
}
