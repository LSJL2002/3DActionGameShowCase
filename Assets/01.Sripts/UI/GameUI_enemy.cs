using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

// GameUI의 Enemy Part
public partial class GameUI : UIBase
{
    public GameObject enemyInfoUI;         // 적UI 최상위 오브젝트 (활성화 컨트롤용 변수)

    public TextMeshProUGUI enemyNameText;  // UI : 적 이름 텍스트
    public TextMeshProUGUI enemyHPText; // UI : 적 체력 텍스트
    public Slider enemyHPSlider;           // UI : 적 체력 슬라이더바

    MonsterStatHandler monsterStats;       // 생성된 몬스터의 stats에 접근가능한 변수

    public void UpdateEnemy()
    {
        //전투 상태일 때만 업데이트
        if (currentBattleState == eBattleState.Battle && monsterStats.isAlive())
        {
            float enemyMaxHP = monsterStats.monsterData.maxHp; // 적 최대체력

            float enemyCurrentHP = monsterStats.CurrentHP; // 적 현재체력
            
            // 적 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
            enemyHPText.text = Mathf.FloorToInt(enemyCurrentHP / enemyMaxHP * 100).ToString() + "%";

            enemyHPSlider.value = enemyCurrentHP / enemyMaxHP; // 적 체력 슬라이더 업데이트
        }
        else if (currentBattleState == eBattleState.Battle)
        {
            ChangeState(eBattleState.Idle);
        }
    }

    // 적 정보 세팅 함수
    public void SetEnemyInfo(int number)
    {
        switch(number)
        {
            case 0:

                enemyNameText.text = null; // 적 이름 변수 클리어

                enemyHPText.text = default; // 적 최대체력 변수 클리어

                enemyHPSlider.maxValue = 1f; // 적 체력 슬라이더를 100%

                enemyInfoUI.SetActive(false); // 적 정보UI 오브젝트를 비활성화

                monsterStats = null; // 스텟 변수 클리어

                break;

            case 1:

                enemyInfoUI.SetActive(true); // 적 정보UI 오브젝트를 활성화

                enemyNameText.text = monsterStats.monsterData.monsterName; // 적 이름 변수 초기화

                enemyHPText.text = monsterStats.monsterData.maxHp.ToString(); // 적 최대체력 변수 초기화

                enemyHPSlider.maxValue = 1f; // 적 체력 슬라이더를 초기화

                break;
        }
    }

    // 적을 로드하는 함수 (이후 맵매니저에서 구현)
    public async void LoadEnemy(string str)
    {
        var monsterInstance = await Addressables.InstantiateAsync(str, new Vector3(0, 0, 0), Quaternion.identity);

        BaseMonster baseMonsterComponent = monsterInstance.GetComponent<BaseMonster>();
        monsterStats = baseMonsterComponent.Stats;

        ChangeState(eBattleState.Battle);
    }
}
