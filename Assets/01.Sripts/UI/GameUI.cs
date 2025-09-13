using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

public enum eBattleState
{
    Idle,
    Battle,
}

// GameUI의 Base
public partial class GameUI : UIBase
{
    private eBattleState currentBattleState;

    protected override void OnEnable()
    {
        base.OnEnable();

        // 상태를 'Idle'로 설정
        ChangeState(eBattleState.Idle);

        // 몬스터 소환
        try { LoadMonster("Test_Monster"); }
        catch { Debug.Log($"{"Test_Monster"} 소환 실패"); }

        OnEnablePlayer();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // 상태를 'Idle'로 설정
        ChangeState(eBattleState.Idle);
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
                try { LoadMonster(str); }
                catch { Debug.Log($"{str} 소환 실패"); }
                break;
        }

        // 현재 팝업창 닫기 (게임상태에서는 UI를 끌 필요 없으므로 일단 주석처리)
        //Hide();
    }

    // 전투 돌입 / 종료시 호출할 함수
    // 돌입시에는 몬스터이름과 최대체력을 매개변수로 받음
    public void ChangeState(eBattleState state, string enemyName = null, float maxHP = default)
    {
        // 매개변수를 받아서 상태를 변경 (Idle <-> Battle)
        currentBattleState = state;

        switch (state)
        {
            case eBattleState.Idle:

                // 적 이름 변수 초기화
                enemyNameText.text = null;

                // 적 최대체력 변수 초기화
                enemyHPText.text = default;

                // 적 체력 슬라이더를 초기화
                enemyHPSlider.maxValue = 1f;

                // 적 정보UI 오브젝트를 비활성화 (동적생성으로 변경 예정)
                enemyInfoUI.SetActive(false);

                monsterStats = null;

                break;

            case eBattleState.Battle:

                // 적 정보UI 오브젝트를 활성화 (동적생성으로 변경 예정)
                enemyInfoUI.SetActive(true);

                // 적 이름 변수 초기화
                enemyNameText.text = enemyName;

                // 적 최대체력 변수 초기화
                enemyHPText.text = maxHP.ToString();

                // 적 체력 슬라이더를 초기화
                enemyHPSlider.maxValue = 1f;

                break;
        }
    }

    // 몬스터 소환 함수(프로토타입 테스트용2 - 삭제예정)
    public async void LoadMonster(string str)
    {
        var monsterInstance = await Addressables.InstantiateAsync(str, new Vector3(0, 0, 0), Quaternion.identity);
        monsterInstance.name = str;

        BaseMonster baseMonsterComponent = monsterInstance.GetComponent<BaseMonster>();
        monsterStats = baseMonsterComponent.Stats;

        string enemyName = monsterStats.monsterData.monsterName;
        float enemyMaxHP = monsterStats.monsterData.maxHp;

        // 배틀매니저의 스테이트를 변경과 동시에 적의 정보를 넘김
        ChangeState(eBattleState.Battle, enemyName, enemyMaxHP);
    }
}
