using System;
using UnityEngine;


//================= 체력 / 에너지 관리 + 회복, 데미지 ===============
public class ResourceModule
{
    public float CurrentHealth { get; private set; }
    public float CurrentEnergy { get; private set; }

    public Stat MaxHealth { get; private set; }
    public Stat MaxEnergy { get; private set; }

    public event Action OnHealthChanged;
    public event Action OnEnergyChanged;
    public event Action OnDie;

    public bool IsDead => CurrentHealth <= 0;

    public ResourceModule(float maxHp, float maxEnergy)
    {
        MaxHealth = new Stat(maxHp);
        MaxEnergy = new Stat(maxEnergy);

        CurrentHealth = MaxHealth.Value;
        CurrentEnergy = MaxEnergy.Value;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        OnHealthChanged?.Invoke();
        Debug.Log($"플레이어 현재체력 : {CurrentHealth}");
        if (CurrentHealth <= 0) OnDie?.Invoke();
    }

    public void RecoverHealth(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth.Value);
        OnHealthChanged?.Invoke();
    }

    public bool UseEnergy(float amount)
    {
        if (CurrentEnergy < amount) return false;
        CurrentEnergy -= amount;
        OnEnergyChanged?.Invoke();
        return true;
    }

    public void RecoverEnergy(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy.Value);
        OnEnergyChanged?.Invoke();
    }
}