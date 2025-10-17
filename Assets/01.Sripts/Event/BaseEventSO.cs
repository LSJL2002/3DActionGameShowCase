using System;
using UnityEngine;

public abstract class BaseEventSO : ScriptableObject 
{
    // 모든 자식 이벤트가 가지게 될 범용적인 Action (리스너가 구독할 대상)
    public event Action OnAnyEventRaised;

    // 자식 클래스가 이벤트를 발생시킬 때 호출할 메서드
    public virtual void Raise()
    {
        OnAnyEventRaised?.Invoke();
    }
}