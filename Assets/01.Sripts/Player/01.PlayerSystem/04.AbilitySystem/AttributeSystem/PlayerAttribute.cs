using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface IStats // 유기
{
    bool IsDead { get; }

    void TakeDamage(float amount);
    void RecoverHealth(float amount);
    void RecoverEnergy(float amount);

    event Action OnDie;
}


//=================== 기본 스탯 + 버프/디버프 관리 ================
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


//================ 각 모듈을 통합, 이벤트 전달, 어빌리티 시스템과 연결 ================
public class PlayerAttribute
{
    public ResourceModule Resource { get; private set; }
    public BufferModule SkillBuffer { get; private set; }
    public BufferModule EvadeBuffer { get; private set; }
    public GaugeModule AwakenGauge { get; private set; }

    public Stat Attack { get; private set; }
    public Stat Defense { get; private set; }
    public Stat MoveSpeed { get; private set; }
    public Stat AttackSpeed { get; private set; }

    public event Action<StatType> OnStatChanged;

    public PlayerAttribute(PlayerInfo info, EventHub hub)
    {
        var data = info.StatData;
        // 스탯
        Attack = new Stat(data.AttackPower);
        Defense = new Stat(data.Defense);
        MoveSpeed = new Stat(data.MoveSpeed);
        AttackSpeed = new Stat(data.AttackSpeed);

        // 체력/에너지
        Resource = new ResourceModule(data.MaxHp, data.MaxMp);
        hub.Connect(Resource);

        // 버퍼
        var skill = info.SkillData.SkillInfoDatas[0];
        SkillBuffer = new BufferModule(1, skill.Cooldown);
        EvadeBuffer = new BufferModule(data.DodgeCount, data.DodgeCooldown);
        hub.Connect(SkillBuffer, true);
        hub.Connect(EvadeBuffer, false);

        // 각성 게이지 (선택적)
        if (info.ModuleData is ModuleData_Yuki yukiData)
        {
            AwakenGauge = new GaugeModule(yukiData.AwakenGaugeCost);
            hub.Connect(AwakenGauge);
        }
    }

    public void Update()
    {
        SkillBuffer.Update();
        EvadeBuffer.Update();
        AwakenGauge.Update();
    }

    // 스탯 증가나 버프 적용
    public void AddModifier(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.Attack: Attack.AddModifier(value); break;
            case StatType.Defense: Defense.AddModifier(value); break;
            case StatType.MoveSpeed: MoveSpeed.AddModifier(value); break;
            case StatType.AttackSpeed: AttackSpeed.AddModifier(value); break;
        }
        OnStatChanged?.Invoke(statType);
    }

    public void RemoveModifier(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.Attack: Attack.RemoveModifier(value); break;
            case StatType.Defense: Defense.RemoveModifier(value); break;
            case StatType.MoveSpeed: MoveSpeed.RemoveModifier(value); break;
            case StatType.AttackSpeed: AttackSpeed.RemoveModifier(value); break;
        }
        OnStatChanged?.Invoke(statType);
    }
}