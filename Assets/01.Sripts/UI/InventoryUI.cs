using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// 아이템 슬롯부분을 제외한 인벤토리 전체의 UI를 보여주는 View 계층의 클래스
// 아이템에 직접 접근할 수는 없고 ViewModel과 이벤트를 통해 아이템이 변경 될 수 있도록 신호를 보내는 역할
public class InventoryUI : UIBase
{
    // 인스펙터에서 직접 할당할 아이템 슬롯 목록
    [SerializeField] private List<ItemSlotUI> itemSlots;

    private InventoryViewModel _viewModel;

    public void Setup(InventoryViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnUpdateUI += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var items = _viewModel.GetItems();
        int slotCount = itemSlots.Count;
        int itemCount = items.Count;

        for (int i = 0; i < slotCount; i++)
        {
            if (i < itemCount)
            {
                // 아이템이 있는 경우 데이터 할당
                itemSlots[i].SetData(items[i].data, items[i].stackCount);
                Debug.Log($"{itemSlots[i]} set");
            }
            else
            {
                // 아이템이 없는 경우 슬롯을 초기화 (내용만 지움)
                itemSlots[i].ClearSlot();
                Debug.Log($"{itemSlots[i]} clear");
            }
        }
    }

    public void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                // UI매니저의 '이전UI' 변수를 찾아 활성화
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                // UI매니저의 '현재UI' 변수에 이전 변수를 저장
                UIManager.Instance.currentUI = UIManager.Instance.previousUI;
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
