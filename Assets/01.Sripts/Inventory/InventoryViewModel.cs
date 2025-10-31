using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Inventory(Model)의 데이터를 받아서 InventoryUI(View)가 쉽게 표시할 수 있도록 가공하고,
// InventoryUI(View)의 사용자 입력을 받아서 Inventory(Model) 모델에 전달
[CreateAssetMenu(fileName = "InventoryViewModel", menuName = "Inventory/InventoryViewModel")]
public class InventoryViewModel : ScriptableObject
{
    private Inventory _model;

    public void Init(Inventory model)
    {
        // 이미 초기화되었다면 중복 구독을 막기 위해 return
        if (_model != null)
            return;

        _model = model;

        // 모델의 각 이벤트에 뷰모델의 메서드를 구독
        EventsManager.Instance.StartListening(GameEvent.OnConsumableItemsChanged, OnConsumableItemsChanged);
        EventsManager.Instance.StartListening(GameEvent.OnSkillItemsChanged, OnSkillItemsChanged);
        EventsManager.Instance.StartListening(GameEvent.OnCoreItemsChanged, OnCoreItemsChanged);
    }

    private void OnDisable()
    {
        if (!Application.isPlaying)
            return;

        if (_model != null)
        {
            if (EventsManager.Instance != null)
            {
                // 구독 해제 로직
                EventsManager.Instance.StopListening(GameEvent.OnConsumableItemsChanged, OnConsumableItemsChanged);
                EventsManager.Instance.StopListening(GameEvent.OnSkillItemsChanged, OnSkillItemsChanged);
                EventsManager.Instance.StopListening(GameEvent.OnCoreItemsChanged, OnCoreItemsChanged);
            }
        }

        _model = null; // 참조 해제
    }

    private void OnConsumableItemsChanged() => EventsManager.Instance.TriggerEvent(GameEvent.OnConsumableUIUpdate);
    private void OnSkillItemsChanged() => EventsManager.Instance.TriggerEvent(GameEvent.OnSkillUIUpdate);
    private void OnCoreItemsChanged() => EventsManager.Instance.TriggerEvent(GameEvent.OnCoreUIUpdate);

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

        DecisionButtonUI decisionButtonUI = UIManager.Instance.Get<DecisionButtonUI>();

        // 이벤트 구독을 위한 델리게이트 변수 생성
        Action<bool> onDecisionMadeCallback = null;
        onDecisionMadeCallback = async (isConfirmed) =>
        {
            // 확인UI에서 허가를 받았다면,
            if (isConfirmed)
            {
                // 현재 허가 내용에 따라 스위치
                switch (UIManager.Instance.currentDecisionState)
                {
                    // 아이템 사용에 대한 내용이라면,
                    case DecisionState.UseItem:
                        // 인벤토리 매니저의 아이템 사용 함수 호출
                        InventoryManager.Instance.UseConsumableItem(itemData);
                        break;

                    // 능력선택에 대한 내용이라면,
                    case DecisionState.SelectAbility:

                        SignalPunnelStep();

                        // 스탯업이라면,
                        if (itemData.itemType == ItemData.ItemType.StatUP)
                        {
                            InventoryManager.Instance.StatUPAbility(itemData);
                        }
                        else
                        {
                            // 인벤토리 매니저의 아이템 추가 함수 호출
                            InventoryManager.Instance.LoadData_Addressables(itemData.name, 1);
                        }

                        //UI매니저에서 '능력선택UI'를 가져와서 끄기
                        UIManager.Instance.Hide<SelectAbilityUI>();
                        //InventoryManager.Instance.LoadData_Addressables("20000000", 5); // 회복물약 5스택 추가
                        //InventoryManager.Instance.LoadData_Addressables("20000002", 5); // 공격력 증가물약 5스택 추가
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

    private void SignalPunnelStep()
    {
        switch (BattleManager.Instance.currentZone.id)
        {
            case 60000000:
                AnalyticsManager.SendFunnelStep("18");
                break;
            case 60000001:
                AnalyticsManager.SendFunnelStep("28");
                break;
            case 60000002:
                AnalyticsManager.SendFunnelStep("28");
                break;
            case 60000003:
                AnalyticsManager.SendFunnelStep("28");
                break;
            case 60000004:
                AnalyticsManager.SendFunnelStep("38");
                break;
            case 60000005:
                AnalyticsManager.SendFunnelStep("38");
                break;
            case 60000006:
                AnalyticsManager.SendFunnelStep("38");
                break;
            case 60000007:
                AnalyticsManager.SendFunnelStep("48");
                break;
            case 60000008:
                AnalyticsManager.SendFunnelStep("48");
                break;
            case 60000009:
                AnalyticsManager.SendFunnelStep("48");
                break;
        }
    }
}