using System;
using TMPro;
using UnityEngine;

// CharacterInfomationUI의 Status Part
public partial class CharacterInfomationUI : UIBase
{
    private PlayerAttribute playerStats; // 플레이어의 stats에 접근가능한 변수
    private Action HealthChanged;
    private Action<StatType> StatChanged;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;

    public void AwakeStatus()
    {
        playerStats = PlayerManager.Instance.Attr;
        HealthChanged = () => { SetPlayerStat(StatType.MaxHealth); };
        StatChanged = SetPlayerStat;
        playerStats.Resource.OnHealthChanged += HealthChanged;
        playerStats.OnStatChanged += StatChanged;
        SetPlayerStat(StatType.MaxHealth);
    }

    // 플레이어 스탯 정보 초기화 함수
    public void SetPlayerStat(StatType statType)
    {
        playerStats = PlayerManager.Instance.Attr;

        healthText.text = $"체력 : {playerStats.Resource.CurrentHealth} / {playerStats.Resource.MaxHealth.Value.ToString()}";
        energyText.text = $"마력 : {playerStats.Resource.CurrentEnergy} / {playerStats.Resource.MaxEnergy.Value.ToString()}";

        attackText.text = $"공격력 : {playerStats.Attack.BaseValue.ToString()} + ({playerStats.Attack.Value.ToString()})";
        defenseText.text = $"방어력 : {playerStats.Defense.BaseValue.ToString()} + ({playerStats.Defense.Value.ToString()})";
        attackSpeedText.text = $"공격속도 : {playerStats.AttackSpeed.BaseValue.ToString()} + ({playerStats.AttackSpeed.Value.ToString()})";
        moveSpeedText.text = $"이동속도 : {playerStats.MoveSpeed.BaseValue.ToString()} + ({playerStats.MoveSpeed.Value.ToString()})";
    }
}
