using System;
using UnityEngine;

public abstract class BaseEventSO<T> : ScriptableObject
{
    // T 타입을 매개변수로 받는 Action
    public event Action<T> OnActionRaised;

    // T 타입 매개변수를 받는 이벤트 발생 메서드
    public void Raise(T value) // value를 매개변수로 받음
    {
        OnActionRaised?.Invoke(value);
    }
}