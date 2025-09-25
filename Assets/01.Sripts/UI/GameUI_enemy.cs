using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

    [SerializeField] private Image enermyHPImage;
    public TextMeshProUGUI enemyNameText;  // UI : 적 이름 텍스트
    public TextMeshProUGUI enemyHPText; // UI : 적 체력 텍스트

    MonsterStatHandler monsterStats;       // 생성된 몬스터의 stats에 접근가능한 변수

    public void OnEnableEnemy()
    {
        BaseMonster.OnEnemyHealthChanged += OnEnemyHealthChanged;
    }

    //public void UpdateEnemy()
    //{
    //    //전투 상태일 때만 업데이트
    //    if (currentBattleState == eBattleState.Battle && monsterStats.isAlive())
    //    {
    //        float enemyMaxHP = monsterStats.monsterData.maxHp; // 적 최대체력

    //        float enemyCurrentHP = monsterStats.CurrentHP; // 적 현재체력
            
    //        // 적 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
    //        enemyHPText.text = Mathf.FloorToInt(enemyCurrentHP / enemyMaxHP * 100).ToString() + "%";

    //        enermyHPImage.fillAmount = enemyCurrentHP / enemyMaxHP; // 적 체력 슬라이더 업데이트
    //    }
    //    else if (currentBattleState == eBattleState.Battle)
    //    {
    //        ChangeState(eBattleState.Idle);
    //    }
    //}

    // 적 정보 세팅 함수
    public void SetEnemyInfo(int number)
    {
        switch(number)
        {
            case 0:

                enemyNameText.text = null; // 적 이름 변수 클리어

                enemyHPText.text = default; // 적 최대체력 변수 클리어

                enermyHPImage.fillAmount = 1f; // 적 체력 슬라이더를 100%

                enemyInfoUI.SetActive(false); // 적 정보UI 오브젝트를 비활성화

                monsterStats = null; // 스텟 변수 클리어

                break;

            case 1:

                enemyInfoUI.SetActive(true); // 적 정보UI 오브젝트를 활성화

                enemyNameText.text = monsterStats.monsterData.monsterName; // 적 이름 변수 초기화

                enemyHPText.text = monsterStats.monsterData.maxHp.ToString("#,##0"); // 적 최대체력 변수 초기화

                enermyHPImage.fillAmount = 1f; // 적 체력 슬라이더를 초기화

                break;
        }
    }

    // 적 체력 변경 이벤트 발생 시 호출
    private void OnEnemyHealthChanged()
    {
        float duration = 0.2f;

        // 기존 닷트윈 애니메이션 중지
        enermyHPImage.DOKill();

        // 닷트윈 체력바 fillAmount를 부드럽게 변경
        enermyHPImage.DOFillAmount(monsterStats.CurrentHP / monsterStats.maxHp, 1.0f) // 0.5초 동안 부드럽게 변경
                       .SetEase(Ease.OutQuad); // 애니메이션 가속/감속 방식

        // 닷트윈 쉐이크 효과
        enermyHPImage.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true);
        enemyHPText.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true);

        // 닷트윈 색상 변경했다 돌아오기 효과
        Color originalColor = enemyHPText.color;                         // 현재 색상 값을 저장
        Sequence mySequence = DOTween.Sequence();                         // 새로운 시퀀스 생성
        mySequence.Append(enemyHPText.DOColor(Color.red, duration));     // 시퀀스에 첫 번째 트윈 추가 (빨간색으로 변경)
        mySequence.Append(enemyHPText.DOColor(originalColor, duration)); // 시퀀스에 두 번째 트윈 추가 (원래 색상으로 돌아오기)

        // 체력 텍스트도 업데이트
        enemyHPText.text = monsterStats.CurrentHP.ToString("#,##0");
    }
}
