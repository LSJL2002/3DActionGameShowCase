using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// GameUI의 Base
public partial class GameUI : UIBase
{
    public enum eBattleState
    {
        Idle,
        Battle,
    }

    private eBattleState currentBattleState;

    protected override void Awake()
    {
        base.Awake();

        ChangeState(eBattleState.Idle); // 상태를 'Idle'로 설정

        OnAwakePlayer();

        OnAwakeSkill();
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
