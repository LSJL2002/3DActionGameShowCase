using System;
using UnityEngine;


// =============== 각성 게이지 관리 ===================
public class GaugeModule
{
    public float Current { get; private set; }
    public float Max { get; private set; }

    private bool isDraining = false;
    private float drainStartValue;
    private float drainDuration;
    private float drainElapsed;

    // 값이 변할 때
    public event Action<float> OnChanged;
    // 가득 찼을 때
    public event Action OnFull;
    // 사용했을 때
    public event Action OnUsed;

    public GaugeModule(float max)
    {
        Max = max;
        Current = 0f;
    }

    // 게이지 증가
    public void Add(float value)
    {
        float prev = Current;
        Current = Mathf.Min(Current + value, Max);
        OnChanged?.Invoke(Current);
        if (Current >= Max)
            OnFull?.Invoke();
    }

    // 게이지 사용
    public void Use(float duration = 10f)
    {
        if (Current <= 0) return;

        isDraining = true;
        drainStartValue = Current;
        drainDuration = duration;
        drainElapsed = 0f;

        OnUsed?.Invoke();
    }

    public void Update()
    {
        if (!isDraining) return;

        drainElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(drainElapsed / drainDuration);
        Current = Mathf.Lerp(drainStartValue, 0f, t);
        OnChanged?.Invoke(Current);

        if (t >= 1f)
            isDraining = false;
    }

    public void Reset()
    {
        Current = 0f;
        OnChanged?.Invoke(Current);
    }
}