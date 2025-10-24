using TMPro;
using UnityEngine;

// CharacterInfomationUI의 Status Part
public partial class CharacterInfomationUI : UIBase
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;

    public void AwakeStatus()
    {
        // 구독해제
        PlayerManager.Instance.Attr.Resource.OnHealthChanged -= SetPlayerStat;
        PlayerManager.Instance.OnActiveCharacterChanged -= OnActiveCharacterChanged;
        EventsManager.Instance.StopListening(GameEvent.OnUsedItem, SetPlayerStat);

        // 구독
        PlayerManager.Instance.Attr.Resource.OnHealthChanged += SetPlayerStat;
        PlayerManager.Instance.OnActiveCharacterChanged += OnActiveCharacterChanged;
        EventsManager.Instance.StartListening(GameEvent.OnUsedItem, SetPlayerStat);

        SetPlayerStat();
    }

    // 캐릭터 변경에 대응하는 함수
    private void OnActiveCharacterChanged(PlayerCharacter newCharacter)
    {
        // 이전 캐릭터의 스탯 이벤트 구독 해제
        EventsManager.Instance.StopListening(GameEvent.OnStatChanged, SetPlayerStat);

        // 새로운 캐릭터의 스탯 이벤트 구독
        EventsManager.Instance.StartListening(GameEvent.OnStatChanged, SetPlayerStat);

        // UI 업데이트
        SetPlayerStat();
    }

    // 플레이어 스탯 정보 초기화 함수
    public void SetPlayerStat()
    {
        PlayerAttribute playerAttr = PlayerManager.Instance.Attr;

        healthText.text = $"체력 : {playerAttr.Resource.CurrentHealth} / {playerAttr.Resource.MaxHealth.Value.ToString()}";
        energyText.text = $"마력 : {playerAttr.Resource.CurrentEnergy} / {playerAttr.Resource.MaxEnergy.Value.ToString()}";
        attackText.text = $"공격력 : {playerAttr.Attack.BaseValue.ToString()} + ({playerAttr.Attack.Value.ToString()})";
        defenseText.text = $"방어력 : {playerAttr.Defense.BaseValue.ToString()} + ({playerAttr.Defense.Value.ToString()})";
        attackSpeedText.text = $"공격속도 : {playerAttr.AttackSpeed.BaseValue.ToString()} + ({playerAttr.AttackSpeed.Value.ToString()})";
        moveSpeedText.text = $"이동속도 : {playerAttr.MoveSpeed.BaseValue.ToString()} + ({playerAttr.MoveSpeed.Value.ToString()})";
    }
}
