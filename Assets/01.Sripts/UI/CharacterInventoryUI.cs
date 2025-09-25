using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// 아이템 슬롯부분을 제외한 인벤토리 전체의 UI를 보여주는 View 계층의 클래스
// 아이템에 직접 접근할 수는 없고 ViewModel과 이벤트를 통해 아이템이 변경 될 수 있도록 신호를 보내는 역할
public class CharacterInventoryUI : UIBase
{
    // 인스펙터에서 직접 할당할 아이템 슬롯 목록
    [SerializeField] private List<ItemSlotUI> itemSlots;

    private InventoryViewModel inventoryViewModel;

    // (구독:인벤토리매니저)
    public static event Action OnUseItemUI;

    protected override void Awake()
    {
        base.Awake();

        OnUseItemUI?.Invoke();

        InventoryManager.Instance.characterInventoryUI = this;

        InventoryManager.Instance.SetInventoryUI();
    }

    public void Setup(InventoryViewModel viewModel)
    {
        inventoryViewModel = viewModel;
        inventoryViewModel.OnConsumableUIUpdate += UpdateUI;
        UpdateUI();
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

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 게임UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<CompanionUI>();
                Hide();
                break;

            // CoreUI로 이동
            case "Left":
                await UIManager.Instance.Show<CharacterCoreUI>();
                Hide();
                break;

            // StatUI로 이동
            case "Right":
                await UIManager.Instance.Show<CharacterStatUI>();
                Hide();
                break;

            case "20000000":
                InventoryManager.Instance.LoadData_Addressables(str, 1);
                break;

            case "20010008":
                InventoryManager.Instance.LoadData_Addressables(str);
                break;

            case "20010005":
                InventoryManager.Instance.LoadData_Addressables(str);
                break;
        }

        // 현재 팝업창 닫기
        //Hide();
    }
}
