using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int Attack;
    public int Defense;
    public int MaxHealth;
    public int MaxStamina;
    public int ShieldBreak;
    public float CriticalChance;
    public float CriticalDamage;
    public int AbilityATK;
    public int AbilityDEF;
    public int Penetration;
    public float StaminaRegen;

    // 두 Stats를 더하는 연산자
    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats
        {
            Attack = a.Attack + b.Attack,
            Defense = a.Defense + b.Defense,
            MaxHealth = a.MaxHealth + b.MaxHealth,
            MaxStamina = a.MaxStamina + b.MaxStamina,
            ShieldBreak = a.ShieldBreak + b.ShieldBreak,
            CriticalChance = a.CriticalChance + b.CriticalChance,
            CriticalDamage = a.CriticalDamage + b.CriticalDamage,
            AbilityATK = a.AbilityATK + b.AbilityATK,
            AbilityDEF = a.AbilityDEF + b.AbilityDEF,
            Penetration = a.Penetration + b.Penetration,
            StaminaRegen = a.StaminaRegen + b.StaminaRegen
        };
    }

    // 두 Stats를 빼는 연산자
    public static Stats operator -(Stats a, Stats b)
    {
        return new Stats
        {
            Attack = a.Attack - b.Attack,
            Defense = a.Defense - b.Defense,
            MaxHealth = a.MaxHealth - b.MaxHealth,
            MaxStamina = a.MaxStamina - b.MaxStamina,
            ShieldBreak = a.ShieldBreak - b.ShieldBreak,
            CriticalChance = a.CriticalChance - b.CriticalChance,
            CriticalDamage = a.CriticalDamage - b.CriticalDamage,
            AbilityATK = a.AbilityATK - b.AbilityATK,
            AbilityDEF = a.AbilityDEF - b.AbilityDEF,
            Penetration = a.Penetration - b.Penetration,
            StaminaRegen = a.StaminaRegen - b.StaminaRegen
        };
    }
}

[CreateAssetMenu(fileName = "NewStatsData", menuName = "Character/StatsData")]
public class DefaultStats : ScriptableObject
{
    [Header("Base Stats")]
    public Stats baseStats = new Stats
    {
        Attack = 55,
        Defense = 30,
        MaxHealth = 700,
        MaxStamina = 100,
        ShieldBreak = 10,
        CriticalChance = 0.1f,
        CriticalDamage = 1.5f,
        AbilityATK = 10,
        AbilityDEF = 10,
        Penetration = 0,
        StaminaRegen = 5f
    };
}