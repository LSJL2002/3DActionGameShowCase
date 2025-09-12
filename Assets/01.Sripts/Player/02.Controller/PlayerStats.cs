using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStats
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    float MaxEnergy { get; }
    float CurrentEnergy { get; }
    int Attack { get; }
    int Defense { get; }

    bool IsDead { get; }

    void TakeDamage(int amount);
    void Heal(int amount);

    event Action OnDie;
}

public class PlayerStats : IStats
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxEnergy { get; private set; }
    public float CurrentEnergy { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    public bool IsDead => CurrentHealth <= 0;


    public event Action OnDie;

    public PlayerStats(PlayerStatsData data)
    {
        MaxHealth = data.maxHp;
        CurrentHealth = MaxHealth;
        MaxEnergy = data.maxHp;
        CurrentEnergy = MaxEnergy;
        Attack = data.attackPower;
        Defense = data.defense;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        if (CurrentHealth == 0) OnDie?.Invoke();
        Debug.Log(CurrentHealth);
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }
}
