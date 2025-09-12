using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum eState
{
    Idle,
    Battle,
}

public class GameUI : UIBase
{
    private eState currentState;

    public GameObject enemyInfoUI;         // 적 오브젝트 UI (활성화 컨트롤용 변수)

    public TextMeshProUGUI playerHPText;   // UI : 플레이어 체력 텍스트
    public Slider playerHPSlider;          // UI : 플레이어 체력 슬라이더바
    public TextMeshProUGUI playerMPText;   // UI : 플레이어 마력 텍스트
    public Slider playerMPSlider;          // UI : 플레이어 마력 슬라이더바
    public TextMeshProUGUI enemyNameText;  // UI : 적 이름 텍스트
    public TextMeshProUGUI enemyMaxHPText; // UI : 적 체력 텍스트
    public Slider enemyHPSlider;           // UI : 적 체력 슬라이더바

    private float playerMaxHP;             // 플레이어 최대 체력
    private float playerMaxMP;             // 플레이어 최대 마력
    private float enemyMaxHP;              // 적 최대 체력

    MonsterStatHandler monsterStats;       // 생성된 몬스터의 stats에 접근가능한 변수

    protected override void OnEnable()
    {
        base.OnEnable();

        // 기본상태를 'Idle'로 설정
        ChangeState(eState.Idle);

        // 플레이어 변수 초기화
        // playerMaxHP = PlayerManager.Instance.
        // playerMaxMP = PlayerManager.Instance.

        // 플레이어 슬라이더를 초기화
        playerHPSlider.maxValue = 1f;
        playerMPSlider.maxValue = 1f;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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

            // Enemy 소환
            case "Test_Monster":

                // 몬스터 소환
                try { LoadMonster(str); }
                catch { Debug.Log($"{str} 소환 실패"); }
                break;
        }

        // 현재 팝업창 닫기 (게임상태에서는 UI를 끌 필요 없으므로 일단 주석처리)
        //Hide();
    }

    protected override void Update()
    {
        base.Update();

        //// 플레이어 현재체력
        //float playerCurrentHP = 0;

        //// 플레이어 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
        //playerHPText.text = Mathf.FloorToInt( playerCurrentHP / playerMaxHP * 100 ).ToString() + "%";

        //// 플레이어 체력 슬라이더 업데이트
        //playerHPSlider.value = playerCurrentHP / playerMaxHP;

        //// 플레이어 최대마력
        //float playerMaxMP = 0;

        //// 플레이어 현재마력
        //float playerCurrentMP = 0;

        //// 플레이어 현재 마력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
        //playerMPText.text = Mathf.FloorToInt( playerCurrentMP / playerMaxMP * 100 ).ToString() + "%";

        //// 플레이어 마력 슬라이더 업데이트
        //playerMPSlider.value = playerCurrentMP / playerMaxMP;

        //전투 상태일 때만 업데이트
        if (currentState == eState.Battle && monsterStats.isAlive())
        {
            // 적 최대체력
            float enemyMaxHP = monsterStats.monsterData.maxHp;

            // 적 현재체력
            float enemyCurrentHP = monsterStats.CurrentHP;

            // 적 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
            enemyMaxHPText.text = Mathf.FloorToInt(enemyCurrentHP / enemyMaxHP * 100).ToString() + "%";

            // 적 체력 슬라이더 업데이트
            enemyHPSlider.value = enemyCurrentHP / enemyMaxHP;
        }
        else
        {
            ChangeState(eState.Idle);
        }
    }

    // 전투 돌입 / 종료시 호출할 함수
    // 돌입시에는 몬스터이름과 최대체력을 매개변수로 받음
    public void ChangeState(eState state, string enemyName = null, float maxHP = default)
    {
        // 매개변수를 받아서 상태를 변경 (Idle <-> Battle)
        currentState = state;

        switch (state)
        {
            case eState.Idle:

                // 적 이름 변수 초기화
                enemyNameText.text = null;

                // 적 최대체력 변수 초기화
                enemyMaxHPText.text = default;

                // 적 체력 슬라이더를 초기화
                enemyHPSlider.maxValue = 1f;

                // 적 정보UI 오브젝트를 비활성화 (동적생성으로 변경 예정)
                enemyInfoUI.SetActive(false);

                monsterStats = null;

                break;

            case eState.Battle:

                // 적 정보UI 오브젝트를 활성화 (동적생성으로 변경 예정)
                enemyInfoUI.SetActive(true);

                // 적 이름 변수 초기화
                enemyNameText.text = enemyName;

                // 적 최대체력 변수 초기화
                enemyMaxHPText.text = maxHP.ToString();

                // 적 체력 슬라이더를 초기화
                enemyHPSlider.maxValue = 1f;

                break;
        }
    }

    public async void LoadMonster(string str)
    {
        GameObject Monster = await ResourceManager.Instance.LoadAsset<GameObject>(str, eAssetType.Monster);
        GameObject monsterInstance = Instantiate(Monster, new Vector3(0, 0, 0), Quaternion.identity);
        monsterInstance.name = str;

        BaseMonster baseMonsterComponent = monsterInstance.GetComponent<BaseMonster>();
        monsterStats = baseMonsterComponent.Stats;

        string enemyName = monsterStats.monsterData.monsterName;
        float enemyMaxHP = monsterStats.monsterData.maxHp;

        // 배틀매니저의 스테이트를 변경과 동시에 적의 정보를 넘김
        ChangeState(eState.Battle, enemyName, enemyMaxHP);
    }
}
