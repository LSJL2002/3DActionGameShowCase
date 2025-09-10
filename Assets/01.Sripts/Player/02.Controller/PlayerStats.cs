using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    [SerializeField] private int maxHealth = 100;
    private int health;

    public bool IsDie => CurrentHealth == 0;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    public event Action OnDie;

    public PlayerStats(PlayerSO data)
    {
        MaxHealth = data.BaseHealth;
        CurrentHealth = MaxHealth;
        Attack = data.BaseAttack;
        Defense = data.BaseDefense;
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
