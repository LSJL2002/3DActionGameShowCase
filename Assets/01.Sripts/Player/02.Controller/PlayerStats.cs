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

    // ================스킬 버퍼/쿨타임================
    public int SkillBufferMax { get; private set; } = 2;
    public int SkillBufferCurrent { get; set; } = 2; //이거랑
    public float SkillCooldown { get; private set; } = 5f; //이거
    public float LastSkillTime { get; set; } = -5f;

    // =================== 회피 관련 ===================
    public int EvadeBufferMax { get; private set; } = 4;
    public int EvadeBufferCurrent { get; set; } = 4;
    public float EvadeCooldown { get; private set; } = 4f;
    public float LastEvadeTime { get; set; } = -4f;



    public bool IsDead => CurrentHealth <= 0;

    public event Action OnDie;

    // 체력이 변경될 때 호출될 이벤트
    public event Action OnPlayerHealthChanged;

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

        // 스킬 버퍼를 풀로 채우고 타이머 초기화
        SkillBufferCurrent = SkillBufferMax;   // 2개로 시작
        LastSkillTime = Time.time;             // 지금 시점을 기준으로 회복 타이머 시작
        // 회피 초기화
        EvadeBufferCurrent = EvadeBufferMax;
        LastEvadeTime = Time.time;
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

    
    // ===================스킬 사용===================
    private bool CanUseSkill()
    {
        bool cooldownReady = Time.time - LastSkillTime >= SkillCooldown;
        return cooldownReady && SkillBufferCurrent > 0;
    }

    public bool UseSkill()
    {
        if (!CanUseSkill()) return false;
        SkillBufferCurrent--;
        LastSkillTime = Time.time;
        return true;
    }


    // =================== 회피 사용 ===================
    private bool CanEvade()
    {
        bool cooldownReady = Time.time - LastEvadeTime >= EvadeCooldown;
        return cooldownReady && EvadeBufferCurrent > 0;
    }

    public bool UseEvade()
    {
        if (!CanEvade()) return false;
        EvadeBufferCurrent--;
        LastEvadeTime = Time.time;
        return true;
    }

    // =================== 버퍼 업데이트 ===================
    public void UpdateEvadeBuffer()
    {
        if (Time.time - LastEvadeTime >= EvadeCooldown && EvadeBufferCurrent < EvadeBufferMax)
        {
            EvadeBufferCurrent++;            // 한 개씩 회복
            LastEvadeTime = Time.time;       // 회복한 시점 기록
        }
    }

    public void UpdateSkillBuffer()
    {
        if (Time.time - LastSkillTime >= SkillCooldown && SkillBufferCurrent < SkillBufferMax)
        {
            SkillBufferCurrent++;            // 한 개씩 증가
            LastSkillTime = Time.time;       // 회복한 시점을 다시 기록 
        }
    }

    // 기존 UpdateSkillBuffer와 같이 외부 Update에서 호출 가능
    public void Update()
    {
        UpdateSkillBuffer();
        UpdateEvadeBuffer();
    }
}