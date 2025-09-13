using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
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
            // 적 최대체력
            float enemyMaxHP = monsterStats.monsterData.maxHp;

            // 적 현재체력
            float enemyCurrentHP = monsterStats.CurrentHP;

            // 적 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
            enemyHPText.text = Mathf.FloorToInt(enemyCurrentHP / enemyMaxHP * 100).ToString() + "%";

            // 적 체력 슬라이더 업데이트
            enemyHPSlider.value = enemyCurrentHP / enemyMaxHP;
        }
        else
        {
            ChangeState(eBattleState.Idle);
        }
    }
}
