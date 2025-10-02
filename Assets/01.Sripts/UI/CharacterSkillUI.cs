using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterSkillUI : UIBase
{
    // 인스펙터에서 직접 할당할 아이템 슬롯 목록
    [SerializeField] private List<ItemSlotUI> itemSlots;

    private InventoryViewModel _viewModel;

    protected override void Awake()
    {
        base.Awake();

        InventoryManager.Instance.characterSkillUI = this;

        InventoryManager.Instance.SetSkillUI();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InventoryManager.Instance.ChangeUseItemState();
    }

    public void Setup(InventoryViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnSkillUIUpdate += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var items = _viewModel.GetSkillItems();
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
