using System;
using UnityEngine;

public class EventHub
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnEnergyChanged;
    public event Action OnPlayerDie;

    public event Action OnSkillBufferChanged;
    public event Action OnEvadeBufferChanged;

    public event Action<float> OnAwakenGaugeChanged;
    public event Action OnAwakenGaugeFull;
    public event Action OnAwakenGaugeUsed;

    // ResourceModule 이벤트 연결
    public void Connect(ResourceModule resource)
    {
        resource.OnDie += () => OnPlayerDie?.Invoke();
        resource.OnHealthChanged += () => OnHealthChanged?.Invoke(resource.CurrentHealth);
        resource.OnEnergyChanged += () => OnEnergyChanged?.Invoke(resource.CurrentEnergy);
    }

    // BufferModule 이벤트 연결
    public void Connect(BufferModule buffer, bool isSkill)
    {
        buffer.OnBufferChanged += () =>
        {
            if (isSkill) OnSkillBufferChanged?.Invoke();
            else OnEvadeBufferChanged?.Invoke();
        };
    }

    // GaugeModule 이벤트 연결
    public void Connect(GaugeModule gauge)
    {
        gauge.OnChanged += (v) => OnAwakenGaugeChanged?.Invoke(v);
        gauge.OnFull += () => OnAwakenGaugeFull?.Invoke();
        gauge.OnUsed += () => OnAwakenGaugeUsed?.Invoke();
    }
}