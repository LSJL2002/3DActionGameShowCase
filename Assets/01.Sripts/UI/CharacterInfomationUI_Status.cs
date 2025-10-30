using TMPro;
using UnityEngine;

// CharacterInfomationUI의 Status Part
public partial class CharacterInfomationUI : UIBase
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;

    public void AwakeStatus()
    {
        // 구독 / 해제
        EventsManager.Instance.StopListening<int>(GameEventT.OnSelectChange, SetPlayerStat);
        EventsManager.Instance.StartListening<int>(GameEventT.OnSelectChange, SetPlayerStat);
    }

    // 플레이어 스탯 정보 초기화 함수
    public void SetPlayerStat(int i)
    {
        PlayerAttribute playerAttr = PlayerManager.Instance.Characters[i].Attr;

        healthText.text = $"체력 : {playerAttr.Resource.CurrentHealth} / {playerAttr.Resource.MaxHealth.Value.ToString()}";
        energyText.text = $"마력 : {playerAttr.Resource.CurrentEnergy} / {playerAttr.Resource.MaxEnergy.Value.ToString()}";
        attackText.text = $"공격력 : {playerAttr.Attack.BaseValue.ToString()} + ({playerAttr.Attack.Value.ToString()})";
        defenseText.text = $"방어력 : {playerAttr.Defense.BaseValue.ToString()} + ({playerAttr.Defense.Value.ToString()})";
        attackSpeedText.text = $"공격속도 : {playerAttr.AttackSpeed.BaseValue.ToString()} + ({playerAttr.AttackSpeed.Value.ToString()})";
        moveSpeedText.text = $"이동속도 : {playerAttr.MoveSpeed.BaseValue.ToString()} + ({playerAttr.MoveSpeed.Value.ToString()})";
    }
}
