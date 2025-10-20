using System;
using System.Collections.Generic;

// 매개변수 없는 타입의 이벤트들
public enum GameEvent
{
    // Inventory
    OnConsumableItemsChanged, // 발생 : 모델(Inventory) , 구독 : 뷰모델(InventoryViewModel)
    OnSkillItemsChanged,      // 발생 : 모델(Inventory) , 구독 : 뷰모델(InventoryViewModel)
    OnCoreItemsChanged,       // 발생 : 모델(Inventory) , 구독 : 뷰모델(InventoryViewModel)
    OnConsumableUIUpdate,     // 발생 : 뷰모델(InventoryViewModel) , 구독 : View(UI)
    OnSkillUIUpdate,          // 발생 : 뷰모델(InventoryViewModel) , 구독 : View(UI)
    OnCoreUIUpdate,           // 발생 : 뷰모델(InventoryViewModel) , 구독 : View(UI)
}

// 매개변수 있는 타입의 이벤트들
public enum GameEventT
{
    OnBattleStart,
    OnBattleClear,
    OnMonsterDie,
}

public class EventsManager : Singleton<EventsManager>
{
    private Dictionary<GameEvent, Action> eventDictionary; // 매개변수 없는 이벤트용 딕셔너리
    private Dictionary<GameEventT, object> eventTDictionary; // 매개변수 있는 이벤트용 딕셔너리

    #region 매개변수 없는 로직
    // enum의 모든 값을 딕셔너리에 미리 등록
    private void Init()
    {
        eventDictionary = new Dictionary<GameEvent, Action>();

        foreach (GameEvent eventType in Enum.GetValues(typeof(GameEvent)))
        {
            if (!eventDictionary.ContainsKey(eventType))
                eventDictionary.Add(eventType, null);
        }
    }

    // 구독
    public void StartListening(GameEvent targetEvent, Action listener)
    {
        if (eventDictionary == null)
            Init();

        if (Instance != null && eventDictionary.ContainsKey(targetEvent))
        {
            eventDictionary[targetEvent] += listener;
        }
    }

    // 구독 해제
    public void StopListening(GameEvent targetEvent, Action listener)
    {
        if (eventDictionary == null) return;

        if (Instance != null && eventDictionary.ContainsKey(targetEvent))
        {
            eventDictionary[targetEvent] -= listener;
        }
    }

    // 이벤트 발생
    public void TriggerEvent(GameEvent targetEvent)
    {
        if (Instance != null && eventDictionary.TryGetValue(targetEvent, out Action thisEvent))
        {
            thisEvent?.Invoke();
        }
    }
    #endregion 매개변수 없는 로직

    // -----------------------------------------------------------------------------------------------------

    #region 매개변수 있는 로직
    private void InitT()
    {
        eventTDictionary = new Dictionary<GameEventT, object>();
    }

    // 구독
    public void StartListening<T>(GameEventT targetEvent, Action<T> listener)
    {
        if (eventTDictionary == null)
            InitT();

        if (eventTDictionary.TryGetValue(targetEvent, out object currentEvent))
        {
            Action<T> thisEvent = currentEvent as Action<T>;
            eventTDictionary[targetEvent] = thisEvent + listener;
        }
        else
        {
            eventTDictionary.Add(targetEvent, listener);
        }
    }

    // 구독 해제
    public void StopListening<T>(GameEventT targetEvent, Action<T> listener)
    {
        if (eventTDictionary == null) return;

        if (eventTDictionary.TryGetValue(targetEvent, out object currentEvent))
        {
            Action<T> thisEvent = currentEvent as Action<T>;
            eventTDictionary[targetEvent] = thisEvent - listener;
        }
    }

    // 이벤트 발생
    public void TriggerEvent<T>(GameEventT targetEvent, T data)
    {
        if (eventTDictionary.TryGetValue(targetEvent, out object currentEvent))
        {
            Action<T> thisEvent = currentEvent as Action<T>;
            thisEvent?.Invoke(data);
        }
    }
    #endregion 매개변수 있는 로직
}