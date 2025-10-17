// GameUI의 Base
using DG.Tweening;
using UnityEngine;

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

        BattleManager.OnBattleStart += LoadMonsterStat;     //전투시작시(ontriggerEnter) 스탯 불러오기
        BattleManager.OnMonsterDie += ReleaseMonsterStat;  //전투끝날시(몬스터사망시) 스텟 해제하기

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

        BattleManager.OnBattleStart -= LoadMonsterStat;    //해제    
        BattleManager.OnBattleClear -= ReleaseMonsterStat; //해제
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
        SetEnemyInfo(1);
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
        SetEnemyInfo(0);
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
