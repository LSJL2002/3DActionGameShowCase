using TMPro;

public class CharacterStatUI : UIBase
{
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI healthText;
    private TextMeshProUGUI currentHealthText;

    public TextMeshProUGUI energyText;
    private TextMeshProUGUI currentEnergyText;

    public TextMeshProUGUI attackText;
    private TextMeshProUGUI extraAttackText; // 추가 공격력

    public TextMeshProUGUI defenseText;
    private TextMeshProUGUI extraDefenseText; // 추가 방어력

    public TextMeshProUGUI attackSpeedText;
    private TextMeshProUGUI extraAttackSpeedText; // 추가 공격속도

    public TextMeshProUGUI moveSpeedText;
    private TextMeshProUGUI extraMoveSpeedText; // 추가 이동속도

    private PlayerStats playerStats; // 플레이어의 stats에 접근가능한 변수

    protected override void OnEnable()
    {
        base.OnEnable();

        SetPlayerStat();
    }

    // 플레이어 정보 초기화 함수
    public void SetPlayerStat()
    {
        playerStats = PlayerManager.Instance.Stats;

        healthText.text = $"체력 : {playerStats.CurrentHealth} / {playerStats.MaxHealth.Value.ToString()}";
        energyText.text = $"마력 : {playerStats.CurrentEnergy} / {playerStats.MaxEnergy.Value.ToString()}";

        attackText.text = $"공격력 : {playerStats.Attack.BaseValue.ToString()} + ({playerStats.Attack.Value.ToString()})"; // 추가 스탯 아직 없음
        defenseText.text = $"방어력 : {playerStats.Defense.BaseValue.ToString()} + ({playerStats.Defense.Value.ToString()})"; // 추가 스탯 아직 없음
        attackSpeedText.text = $"공격속도 : {playerStats.AttackSpeed.BaseValue.ToString()} + ({playerStats.AttackSpeed.Value.ToString()})"; // 플레이어 스탯에 공격속도 스탯 아직 없음
        moveSpeedText.text = $"이동속도 : {playerStats.MoveSpeed.BaseValue.ToString()} + ({playerStats.MoveSpeed.Value.ToString()})"; // 플레이어 스탯에 이동속도 스탯 아직 없음
    }

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 게임UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<TownUI>();
                break;

            // 인벤토리 UI 켜기
            case "Left":
                await UIManager.Instance.Show<CharacterInventoryUI>();
                break;

            // 캐릭터 스킬 UI 켜기
            case "Right":
                await UIManager.Instance.Show<CharacterSkillUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
