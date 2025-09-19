using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

// GameUI의 Base
public partial class GameUI : UIBase
{
    public enum eBattleState
    {
        Idle,
        Battle,
    }

    private eBattleState currentBattleState;

    protected override void OnEnable()
    {
        base.OnEnable();

        ChangeState(eBattleState.Idle); // 상태를 'Idle'로 설정

        OnEnablePlayer();

        // 몬스터 소환
        //try { LoadEnemy("Test_Monster"); }
        //catch { Debug.Log($"{"Test_Monster"} 소환 실패"); }
        BattleManager.OnBattleStart += LoadMonsterStat;     //전투시작시(ontriggerEnter) 스탯불러오기
        BattleManager.OnBattleClear += ReleaseMonsterStat;  //전투끝날시(몬스터사망시) 스텟 해제하기
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ChangeState(eBattleState.Idle); // 상태를 'Idle'로 설정
        BattleManager.OnBattleStart -= LoadMonsterStat;    //해제    
        BattleManager.OnBattleClear -= ReleaseMonsterStat; //해제
    }

    protected override void Update()
    {
        base.Update();

        UpdatePlayer();

        UpdateEnemy();
    }

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "Pause":

                // 게임매니저의 게임 일시정지 메서드를 호출
                GameManager.Instance.PauseGame(true);
                // 일시정지 UI 팝업
                await UIManager.Instance.Show<PauseUI>();

                break;

            // Enemy 소환 (프로토타입 테스트용1)
            case "Test_Monster":

                // 몬스터 소환
                try { LoadEnemy("Test_Monster"); }
                catch { Debug.Log($"{"Test_Monster"} 소환 실패"); }
                break;
        }
    }

    public void LoadMonsterStat(BattleZone zone)                     //몬스터 스텟 불러오기
    {
        monsterStats = BattleManager.Instance.monsterStats;
        ChangeState(eBattleState.Battle);
        SetEnemyInfo(1);
    }

    public void ReleaseMonsterStat(BattleZone zone)                  //몬스터 스텟 해제
    {
        monsterStats = null;
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
