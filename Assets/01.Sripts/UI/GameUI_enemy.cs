using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// GameUI의 Enemy Part
public partial class GameUI : UIBase
{
    public enum eBattleState
    {
        Idle,
        Battle,
    }

    [Header("[Enemy]")]
    [SerializeField] private Image enemyHPImage_Back;
    [SerializeField] private Image enemyHPImage_Front;
    [SerializeField] public TextMeshProUGUI enemyNameText;  // UI : 적 이름 텍스트
    [SerializeField] public TextMeshProUGUI enemyHPText; // UI : 적 체력 텍스트
    [SerializeField] private CanvasGroup enemyInfoCanvasGroup;

    private eBattleState currentBattleState;
    private MonsterStatHandler monsterStats;       // 생성된 몬스터의 stats에 접근가능한 변수
    private Sequence enemyDamageSequence;
    private Color defaultColor = new Color32 (255, 141, 40, 200);

    public void OnEnableEnemy()
    {
        enemyInfoCanvasGroup.alpha = 0f;
        ChangeState(eBattleState.Idle); // 상태를 'Idle'로 설정

        // 구독해제
        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleStart, LoadMonsterStat);
        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleClear, ReleaseMonsterStat);

        // 구독
        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleStart, LoadMonsterStat);
        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleClear, ReleaseMonsterStat);
    }

    // 적 정보 세팅 함수
    public void SetEnemyInfo(int number)
    {
        if (enemyInfoCanvasGroup == null
            || enemyHPImage_Back == null
            || enemyHPImage_Front == null
            || enemyHPText == null
            || enemyNameText == null)
        {
            Debug.LogWarning("[GameUI] Enemy UI elements are missing — skipping SetEnemyInfo()");
            return;
        }
        switch (number)
        {
            case 0:

                enemyNameText.text = null; // 적 이름 변수 클리어
                enemyHPText.text = default; // 적 최대체력 변수 클리어
                enemyHPImage_Back.fillAmount = 1f; // 적 체력 슬라이더를 100%
                enemyInfoCanvasGroup.DOFade(0f, 0f);
                break;

            case 1:

                enemyInfoCanvasGroup.DOFade(1f, 1.5f);
                enemyNameText.text = monsterStats.monsterData.monsterName; // 적 이름 변수 초기화
                enemyHPText.text = monsterStats.monsterData.maxHp.ToString("#,##0"); // 적 최대체력 변수 초기화
                enemyHPImage_Front.color = defaultColor;
                enemyHPImage_Back.fillAmount = 1f; // 적 체력 슬라이더를 초기화
                enemyHPImage_Front.fillAmount = 1f; // 적 체력 슬라이더를 초기화

                break;
        }
    }

    public void OnDisableEnemy()
    {
        DOTween.Kill(this);
        enemyDamageSequence = null;
    }

    public void LoadMonsterStat(BattleZone zone)                     //몬스터 스텟 불러오기
    {
        monsterStats = BattleManager.Instance.monsterStats;
        ChangeState(eBattleState.Battle);
        EventsManager.Instance.StartListening(GameEvent.OnHealthChanged, OnEnemyHealthChanged);
    }

    public void ReleaseMonsterStat(BattleZone zone)                  //몬스터 스텟 해제
    {
        if (monsterStats != null)
        {
            EventsManager.Instance.StopListening(GameEvent.OnHealthChanged, OnEnemyHealthChanged);
            monsterStats = null;
        }
        ChangeState(eBattleState.Idle);
    }

    // 적 체력 변경 이벤트 발생 시 호출
    private void OnEnemyHealthChanged()
    {
        // 체력 텍스트 업데이트
        float monsterCurrentHealth = Mathf.Max(monsterStats.CurrentHP, 0); // 0이하로 안내려가게
        enemyHPText.text = monsterCurrentHealth.ToString("#,##0");

        enemyDamageSequence.Kill(); // 이전 시퀀스가 실행중이면 종료

        float duration = 0.2f;

        // 닷트윈 색상 변경했다 돌아오기 효과
        enemyDamageSequence = DOTween.Sequence();
        enemyDamageSequence.Append(enemyHPText.DOColor(Color.red, duration));
        enemyDamageSequence.Append(enemyHPText.DOColor(Color.white, duration));
        enemyDamageSequence.Append(enemyHPImage_Front.DOFillAmount(monsterStats.CurrentHP / monsterStats.maxHp, 0f));
        enemyDamageSequence.Append(enemyHPImage_Back.DOFillAmount(monsterStats.CurrentHP / monsterStats.maxHp, 3.0f).SetEase(Ease.OutQuad));
    }

    // 전투종료시 Idle로 호출할 함수
    public void ChangeState(eBattleState state)
    {
        // 매개변수를 받아서 상태를 변경 (Idle <-> Battle)
        currentBattleState = state;

        switch (state)
        {
            case eBattleState.Idle:

                SetEnemyInfo(0);

                break;

            case eBattleState.Battle:

                SetEnemyInfo(1);

                break;
        }
    }
}
