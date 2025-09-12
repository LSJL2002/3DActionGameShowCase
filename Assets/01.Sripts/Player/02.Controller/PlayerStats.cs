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
    [SerializeField] private int maxHealth = 100;
    private int health;

    public bool IsDead => CurrentHealth == 0;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxEnergy { get; private set; }
    public float CurrentEnergy { get; private set; }

    public int Attack { get; private set; }
    public int Defense { get; private set; }

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

    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (CurrentHealth == 0) OnDie?.Invoke();
        Debug.Log(health);
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }
}
