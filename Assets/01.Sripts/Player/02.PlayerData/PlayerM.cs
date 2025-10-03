using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using static Stats;
using Random = System.Random;
using Time = UnityEngine.Time;

public interface IPlayerM //참조하는이들이 내부구조 몰라도됨, 테스트또는 몬스터등 재사용가능
{
    void Heal(int amount);
    void GainExp(int amount);
    void AddStats(Stats stats);
    void RemoveStats(Stats stats);
}

public sealed class PlayerM : IPlayerM
{
    // --- ReactiveProperty 필드 ---
    private readonly ReactiveProperty<int> _health;
    private readonly ReactiveProperty<int> _stamina;
    private readonly ReactiveProperty<int> _attack;
    private readonly ReactiveProperty<int> _defense;
    private readonly ReactiveProperty<int> _level;
    private readonly ReactiveProperty<int> _exp;

    private readonly ReactiveProperty<int> _maxHealth;
    private readonly ReactiveProperty<int> _maxStamina;
    private readonly ReactiveProperty<int> _shieldBreak;
    private readonly ReactiveProperty<float> _criticalChance;
    private readonly ReactiveProperty<float> _criticalDamage;
    private readonly ReactiveProperty<int> _abilityATK;
    private readonly ReactiveProperty<int> _abilityDEF;
    private readonly ReactiveProperty<int> _penetration;
    private readonly ReactiveProperty<float> _staminaRegen;

    // --- 외부에서 읽기만 가능한 프로퍼티 ---
    public IReadOnlyReactiveProperty<int> Health => _health;
    public IReadOnlyReactiveProperty<int> Stamina => _stamina;
    public IReadOnlyReactiveProperty<int> Attack => _attack;
    public IReadOnlyReactiveProperty<int> Defense => _defense;
    public IReadOnlyReactiveProperty<int> Level => _level;
    public IReadOnlyReactiveProperty<int> Exp => _exp;
    public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;
    public IReadOnlyReactiveProperty<int> MaxStamina => _maxStamina;
    public IReadOnlyReactiveProperty<int> ShieldBreak => _shieldBreak;
    public IReadOnlyReactiveProperty<float> CriticalChance => _criticalChance;
    public IReadOnlyReactiveProperty<float> CriticalDamage => _criticalDamage;
    public IReadOnlyReactiveProperty<int> AbilityATK => _abilityATK;
    public IReadOnlyReactiveProperty<int> AbilityDEF => _abilityDEF;
    public IReadOnlyReactiveProperty<int> Penetration => _penetration;
    public IReadOnlyReactiveProperty<float> StaminaRegenRate => _staminaRegen;


    private Stats baseStats;   // 캐릭터 기본 스텟
    private Stats bonusStats;  // 장비/버프 합산
    private Stats effectiveStats => baseStats + bonusStats;


    public event Action<int> OnLevelUp;


    public PlayerM(DefaultStats statsData)
    {
        baseStats = statsData.baseStats;
        bonusStats = new Stats();

        // ReactiveProperty 생성
        _health = new ReactiveProperty<int>(baseStats.MaxHealth);
        _stamina = new ReactiveProperty<int>(baseStats.MaxStamina);
        _attack = new ReactiveProperty<int>(effectiveStats.Attack);
        _defense = new ReactiveProperty<int>(effectiveStats.Defense);
        _level = new ReactiveProperty<int>(1);
        _exp = new ReactiveProperty<int>(0);

        _maxHealth = new ReactiveProperty<int>(effectiveStats.MaxHealth);
        _maxStamina = new ReactiveProperty<int>(effectiveStats.MaxStamina);
        _shieldBreak = new ReactiveProperty<int>(effectiveStats.ShieldBreak);
        _criticalChance = new ReactiveProperty<float>(effectiveStats.CriticalChance);
        _criticalDamage = new ReactiveProperty<float>(effectiveStats.CriticalDamage);
        _abilityATK = new ReactiveProperty<int>(effectiveStats.AbilityATK);
        _abilityDEF = new ReactiveProperty<int>(effectiveStats.AbilityDEF);
        _penetration = new ReactiveProperty<int>(effectiveStats.Penetration);
        _staminaRegen = new ReactiveProperty<float>(effectiveStats.StaminaRegen);

        Exp.Subscribe(_ => CheckLevelUp());
    }

    private void CheckLevelUp()
    {
        int expForNextLevel = _level.Value * 100;
        while (_exp.Value >= expForNextLevel)
        {
            _exp.Value -= expForNextLevel;
            _level.Value++;

            _maxHealth.Value += 10;
            _maxStamina.Value += 20;
            _attack.Value += 2;
            _defense.Value += 1;
            _criticalChance.Value += 0.01f;
            _criticalDamage.Value += 0.05f;
            _penetration.Value += 1;
            _staminaRegen.Value += 0.5f;

            _health.Value = _maxHealth.Value;
            _stamina.Value = _maxStamina.Value;

            expForNextLevel = _level.Value * 100;
            OnLevelUp?.Invoke(_level.Value);
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        _health.Value = Math.Min(_maxHealth.Value, _health.Value + amount);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        _health.Value = Math.Max(0, _health.Value - amount);
    }

    public void UseStamina(int amount)
    {
        if (amount <= 0) return;
        _stamina.Value = Math.Max(0, _stamina.Value - amount);
    }

    public bool CanUseStamina(int amount) => _stamina.Value >= amount;


    private float staminaAccum;
    public void RecoverStamina()
    {
        staminaAccum += _staminaRegen.Value * Time.deltaTime;
        int recover = (int)Math.Floor(staminaAccum);
        if (recover > 0)
        {
            _stamina.Value = Math.Min(_maxStamina.Value, _stamina.Value + recover);
            staminaAccum -= recover;
        }
    }

    // 공격 계산 (치명타, 관통력 반영)
    private Random rng = new Random();
    public struct DamageResult
    {
        public int Damage;
        public bool IsCritical;
    }

    public DamageResult CalculateDamage(int targetDefense)
    {
        bool isCrit = rng.NextDouble() < _criticalChance.Value;
        float damage = _attack.Value - Mathf.Max(0, targetDefense - _penetration.Value);
        if (damage < 1) damage = 1;
        if (isCrit) damage *= _criticalDamage.Value;

        return new DamageResult
        {
            Damage = (int)Math.Round(damage),
            IsCritical = isCrit
        };
    }

    // 공격받을떄
    public void ReceiveAttack(PlayerM attacker)
    {
        var result = attacker.CalculateDamage(_defense.Value);
        TakeDamage(result.Damage);
        // 필요 시 UI용으로 IsCritical 사용 가능
    }

    // --- 경험치 획득 ---
    public void GainExp(int amount)
    {
        if (amount <= 0) return;
        _exp.Value += amount;
    }

    public void AddStats(Stats stats)
    {
        bonusStats += stats;
        UpdateReactiveProperties();
    }

    public void RemoveStats(Stats stats)
    {
        bonusStats -= stats;
        UpdateReactiveProperties();
        _health.Value = Math.Min(_health.Value, _maxHealth.Value);
        _stamina.Value = Math.Min(_stamina.Value, _maxStamina.Value);
    }

    private void UpdateReactiveProperties()
    {
        _attack.Value = effectiveStats.Attack;
        _defense.Value = effectiveStats.Defense;
        _maxHealth.Value = effectiveStats.MaxHealth;
        _maxStamina.Value = effectiveStats.MaxStamina;
        _shieldBreak.Value = effectiveStats.ShieldBreak;
        _criticalChance.Value = effectiveStats.CriticalChance;
        _criticalDamage.Value = effectiveStats.CriticalDamage;
        _abilityATK.Value = effectiveStats.AbilityATK;
        _abilityDEF.Value = effectiveStats.AbilityDEF;
        _penetration.Value = effectiveStats.Penetration;
        _staminaRegen.Value = effectiveStats.StaminaRegen;
    }
}