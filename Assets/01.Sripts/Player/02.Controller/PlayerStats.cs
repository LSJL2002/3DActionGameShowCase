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

    // ====================스킬 버퍼======================
    public int SkillBufferMax { get; private set; } = 2;
    public int SkillBufferCurrent { get; set; }
    public float SkillCooldown { get; private set; } = 5f;

    // =================== 회피 버퍼 =====================
    public int EvadeBufferMax { get; private set; } = 4;
    public int EvadeBufferCurrent { get; set; }
    public float EvadeCooldown { get; private set; } = 4f;

    // ===================누적 타이머======================
    private readonly Queue<float> skillRecoveryTimes = new();
    private readonly Queue<float> evadeRecoveryTimes = new();
    // 마지막으로 큐에 넣은 회복 시간(연속 회복을 위해)
    private float lastSkillRecoveryTime = 0f;
    private float lastEvadeRecoveryTime = 0f;


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
        MoveSpeed = new Stat(data.moveSpeed);
        AttackSpeed = new Stat(data.attackSpeed);

        CurrentHealth = MaxHealth.Value;
        CurrentEnergy = MaxEnergy.Value;

        SkillBufferCurrent = SkillBufferMax; // 스킬 버퍼를 풀로 채우고 타이머 초기화
        EvadeBufferCurrent = EvadeBufferMax; // 회피 초기화

        // 초기 last times를 현재 시간으로 세팅해도 되고 0으로 둬도 됨
        lastSkillRecoveryTime = 0f;
        lastEvadeRecoveryTime = 0f;
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
        OnPlayerHealthChanged?.Invoke(); // 체력이 변경될 때 이벤트 호출
    }

    public void RecoverEnergy(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy.Value);
        OnPlayerHealthChanged?.Invoke(); // 체력이 변경될 때 이벤트 호출
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
            case StatType.MoveSpeed: MoveSpeed.RemoveModifier(value); break;
            case StatType.AttackSpeed: AttackSpeed.RemoveModifier(value); break;
        }
        OnStatChanged?.Invoke(statType);
    }

    public event Action<StatType> OnStatChanged;


    // ===================버퍼 사용===================
    public bool UseSkill()
    {
        if (SkillBufferCurrent <= 0) return false;

        SkillBufferCurrent--;

        // Sequential recovery time logic
        float nextRecover;
        if (skillRecoveryTimes.Count == 0)
        {
            nextRecover = Time.time + SkillCooldown;
        }
        else
        {
            // ensure chained spacing: last + cooldown
            nextRecover = lastSkillRecoveryTime + SkillCooldown;
            // but also ensure it's at least now + cooldown
            if (nextRecover < Time.time + SkillCooldown)
                nextRecover = Time.time + SkillCooldown;
        }

        skillRecoveryTimes.Enqueue(nextRecover);
        lastSkillRecoveryTime = nextRecover;

        return true;
    }

    public bool UseEvade()
    {
        if (EvadeBufferCurrent <= 0) return false;

        EvadeBufferCurrent--;

        float nextRecover;
        if (evadeRecoveryTimes.Count == 0)
        {
            nextRecover = Time.time + EvadeCooldown;
        }
        else
        {
            nextRecover = lastEvadeRecoveryTime + EvadeCooldown;
            if (nextRecover < Time.time + EvadeCooldown)
                nextRecover = Time.time + EvadeCooldown;
        }

        evadeRecoveryTimes.Enqueue(nextRecover);
        lastEvadeRecoveryTime = nextRecover;

        return true;
    }

    // =================== 버퍼 업데이트 ===================
    public void UpdateSkillBuffer()
    {
        while (skillRecoveryTimes.Count > 0 && skillRecoveryTimes.Peek() <= Time.time)
        {
            skillRecoveryTimes.Dequeue();
            if (SkillBufferCurrent < SkillBufferMax)
                SkillBufferCurrent++;
        }
    }

    public void UpdateEvadeBuffer()
    {
        while (evadeRecoveryTimes.Count > 0 && evadeRecoveryTimes.Peek() <= Time.time)
        {
            evadeRecoveryTimes.Dequeue();
            if (EvadeBufferCurrent < EvadeBufferMax)
                EvadeBufferCurrent++;
        }
    }
    // 기존 UpdateSkillBuffer와 같이 외부 Update에서 호출 가능
    public void Update()
    {
        UpdateSkillBuffer();
        UpdateEvadeBuffer();
    }
}