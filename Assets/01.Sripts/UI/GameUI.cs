using DG.Tweening;
using UnityEngine;

// GameUI의 Base
public partial class GameUI : UIBase
{
    public enum eBattleState
    {
        Idle,
        Battle,
    }

    private eBattleState currentBattleState;

    [SerializeField] CanvasGroup gameUICanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        ChangeState(eBattleState.Idle); // 상태를 'Idle'로 설정

        OnAwakePlayer();

        OnAwakeSkill();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => 
        {
            // 각 UI 알파값 1로 변경(페이드인 효과)
            gameUICanvasGroup.DOFade(1f, 1f);
        });
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        uiType = UIType.GameUI;

        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleStart, LoadMonsterStat); // 구독
        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleClear, ReleaseMonsterStat); // 구독

        OnEnablePlayer();
        OnEnableEnemy();
        OnEnableSkill();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnDisablePlayer();
        OnDisableEnemy();
        OnDisableSkill();

        if(EventsManager.Instance != null)
        {
            EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleStart, LoadMonsterStat); // 구독해제
            EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleClear, ReleaseMonsterStat); // 구독해제
        }
    }

    protected override void Update()
    {
        base.Update();

        OnUpdateSkill();
    }

    public void LoadMonsterStat(BattleZone zone)                     //몬스터 스텟 불러오기
    {
        monsterStats = BattleManager.Instance.monsterStats;
        ChangeState(eBattleState.Battle);
        monsterStats.OnHealthChanged += OnEnemyHealthChanged;
    }

    public void ReleaseMonsterStat(BattleZone zone)                  //몬스터 스텟 해제
    {
        if (monsterStats != null)
        {
            monsterStats.OnHealthChanged -= OnEnemyHealthChanged;
            monsterStats = null;
        }
        ChangeState(eBattleState.Idle);
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
