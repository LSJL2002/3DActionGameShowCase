using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCoreUI : UIBase
{
    // 인스펙터에서 직접 할당할 아이템 슬롯 목록
    [SerializeField] private List<ItemSlotUI> itemSlots;

    private InventoryViewModel _viewModel;

    protected override void Awake()
    {
        base.Awake();

        InventoryManager.Instance.CharacterCoreUI = this;

        InventoryManager.Instance.SetCoreUI();
    }

    public void Setup(InventoryViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnCoreUIUpdate += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var items = _viewModel.GetCoreItems();
        int slotCount = itemSlots.Count;
        int itemCount = items.Count;

        for (int i = 0; i < slotCount; i++)
        {
            if (i < itemCount)
            {
                // 아이템이 있는 경우 데이터 할당
                itemSlots[i].SetData(_viewModel, items[i].data, items[i].stackCount);
            }
            else
            {
                // 아이템이 없는 경우 슬롯을 초기화 (내용만 지움)
                itemSlots[i].ClearSlot();
            }
        }
    }

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 게임UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<TownUI>();
                break;

            // 스킬UI로 이동
            case "Left":
                await UIManager.Instance.Show<CharacterSkillUI>();
                break;

            // 인벤토리UI로 이동
            case "Right":
                await UIManager.Instance.Show<CharacterInventoryUI>();
                break;
        }
        
        // 현재 팝업창 닫기
        Hide();
    }

    // 스킬장착 함수 (스킬카드 버튼에서 호출)
    // 이미 장착상태일시 해제되도록 로직 구성
    public void SetSkill(string str)
    {
        
    }
}
