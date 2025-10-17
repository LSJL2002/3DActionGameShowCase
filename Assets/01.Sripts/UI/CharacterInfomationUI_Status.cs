using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// CharacterInfomationUI의 Status Part
public partial class CharacterInfomationUI : UIBase
{
    private PlayerAttribute playerStats; // 플레이어의 stats에 접근가능한 변수

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;

    public void AwakeStatus()
    {
        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독해제 (중복구독 방지)
        PlayerManager.Instance.Stats.OnPlayerHealthChanged -= () => SetPlayerStat(StatType.MaxHealth);
        PlayerManager.Instance.Stats.OnStatChanged -= SetPlayerStat;

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독
        PlayerManager.Instance.Stats.OnPlayerHealthChanged += () => SetPlayerStat(StatType.MaxHealth);
        PlayerManager.Instance.Stats.OnStatChanged += SetPlayerStat;

        // 초기 UI 갱신
        playerStats = PlayerManager.Instance.Stats;
        SetPlayerStat(StatType.MaxHealth);
    }

    // 플레이어 스탯 정보 초기화 함수
    public void SetPlayerStat(StatType statType)
    {
        playerStats = PlayerManager.Instance.Stats;

        healthText.text = $"체력 : {playerStats.CurrentHealth} / {playerStats.MaxHealth.Value.ToString()}";
        energyText.text = $"마력 : {playerStats.CurrentEnergy} / {playerStats.MaxEnergy.Value.ToString()}";

        attackText.text = $"공격력 : {playerStats.Attack.BaseValue.ToString()} + ({playerStats.Attack.Value.ToString()})";
        defenseText.text = $"방어력 : {playerStats.Defense.BaseValue.ToString()} + ({playerStats.Defense.Value.ToString()})";
        attackSpeedText.text = $"공격속도 : {playerStats.AttackSpeed.BaseValue.ToString()} + ({playerStats.AttackSpeed.Value.ToString()})";
        moveSpeedText.text = $"이동속도 : {playerStats.MoveSpeed.BaseValue.ToString()} + ({playerStats.MoveSpeed.Value.ToString()})";
    }
}
