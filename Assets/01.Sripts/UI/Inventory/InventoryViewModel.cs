using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;

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

    // 아이템 사용 (아이템슬롯UI(View)에서 호출)
    public async void SelectItem(ItemData itemData)
    {
        DecisionButtonUI decisionUI = await UIManager.Instance.Show<DecisionButtonUI>();

        // 이벤트 구독을 위한 델리게이트 변수 생성
        Action<bool> onDecisionMadeCallback = null;
        onDecisionMadeCallback = async (isConfirmed) =>
        {
            // 확인UI에서 허가를 받았다면,
            if (isConfirmed)
            {
                // 현재 허가 내용에 따라 스위치
                switch (InventoryManager.Instance.currentDecisionState)
                {
                    // 아이템 사용에 대한 내용이라면,
                    case DecisionState.UseItem:
                        // 인벤토리 매니저의 아이템 사용 함수 호출
                        InventoryManager.Instance.UseConsumableItem(itemData);
                        break;

                    // 능력선택에 대한 내용이라면,
                    case DecisionState.SelectAbility:
                        
                        // 스탯업이라면,
                        if (itemData.itemType == ItemData.ItemType.StatUP)
                        {
                            InventoryManager.Instance.StatUPAbility(itemData);
                        }                        
                        else
                        {
                            // 인벤토리 매니저의 아이템 추가 함수 호출
                            InventoryManager.Instance.LoadData_Addressables(itemData.name);
                        }
                        
                        //UI매니저에서 '능력선택UI'를 가져와서 끄기
                        UIManager.Instance.Hide<SelectAbilityUI>();

                        await TimeLineManager.Instance.OnTimeLine<PlayableDirector>("TimeLine_DrainAbility");
                        break;
                }
            }

            // 이벤트 구독 해제
            decisionUI.OnDecisionMade -= onDecisionMadeCallback;
        };

        // 이벤트 구독
        decisionUI.OnDecisionMade += onDecisionMadeCallback;
    }
}