using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface IStats
{
    bool IsDead { get; }

    void TakeDamage(float amount);
    void RecoverHealth(float amount);
    void RecoverEnergy(float amount);

    event Action OnDie;
}

//버프효과, %증가, 증가/감소이벤트

public class Stat
{
    public float BaseValue { get; private set; } //레벨업 시스템은 base증가하면됨
    private readonly List<float> modifiers = new();

    public float Value => BaseValue + modifiers.Sum();

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
    }

    public void AddModifier(float value) => modifiers.Add(value);
    public void RemoveModifier(float value) => modifiers.Remove(value);
}

public enum StatType { MaxHealth, MaxEnergy, Attack, Defense, MoveSpeed, AttackSpeed }

public class PlayerStats : IStats
{
    public Stat MaxHealth { get; private set; }
    public Stat MaxEnergy { get; private set; }
    public Stat Attack { get; private set; }
    public Stat Defense { get; private set; }
    public Stat MoveSpeed { get; private set; }
    public Stat AttackSpeed { get; private set; }
    public float CurrentHealth { get; private set; }
    public float CurrentEnergy { get; private set; }



    public bool IsDead => CurrentHealth <= 0;

    public event Action OnDie;

    // 체력이 변경될 때 호출될 이벤트
    public event System.Action OnPlayerHealthChanged;

    public PlayerStats(PlayerStatData data)
    {
        MaxHealth = new Stat(data.maxHp);
        MaxEnergy = new Stat(data.maxMp);
        Attack = new Stat(data.attackPower);
        Defense = new Stat(data.defense);
        MoveSpeed = new Stat(data.Speed);
        AttackSpeed = new Stat(data.attackSpeed);

        CurrentHealth = MaxHealth.Value;
        CurrentEnergy = MaxEnergy.Value; 
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        if (CurrentHealth <= 0) OnDie?.Invoke();

        OnPlayerHealthChanged?.Invoke(); // 체력이 변경될 때 이벤트 호출
    }

    public bool UseEnergy(float amount)
    {
        if (CurrentEnergy < amount) return false;
        CurrentEnergy -= amount;
        return true;
    }

    public void RecoverHealth(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth.Value);
    }

    public void RecoverEnergy(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy.Value);
    }

    public void AddModifier(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHealth: MaxHealth.AddModifier(value); break;
            case StatType.MaxEnergy: MaxEnergy.AddModifier(value); break;
            case StatType.Attack: Attack.AddModifier(value); break;
            case StatType.Defense: Defense.AddModifier(value); break;
            case StatType.MoveSpeed: MoveSpeed.AddModifier(value); break;
            case StatType.AttackSpeed: AttackSpeed.AddModifier(value); break;
        }
        // 필요 시 이벤트 호출
        OnStatChanged?.Invoke(statType);
    }

    public void RemoveModifier(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHealth: MaxHealth.RemoveModifier(value); break;
            case StatType.MaxEnergy: MaxEnergy.RemoveModifier(value); break;
            case StatType.Attack: Attack.RemoveModifier(value); break;
            case StatType.Defense: Defense.RemoveModifier(value); break;
            case StatType.MoveSpeed: MoveSpeed.AddModifier(value); break;
            case StatType.AttackSpeed: AttackSpeed.AddModifier(value); break;
        }
        OnStatChanged?.Invoke(statType);
    }

    public event Action<StatType> OnStatChanged;
}
