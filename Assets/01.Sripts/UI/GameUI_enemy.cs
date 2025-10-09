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

// GameUI의 Enemy Part
public partial class GameUI : UIBase
{
    [SerializeField] private Image enemyHPImage_Back;
    [SerializeField] private Image enemyHPImage_Front;
    [SerializeField] public TextMeshProUGUI enemyNameText;  // UI : 적 이름 텍스트
    [SerializeField] public TextMeshProUGUI enemyHPText; // UI : 적 체력 텍스트
    [SerializeField] private CanvasGroup enemyInfoCanvasGroup;
    
    private MonsterStatHandler monsterStats;       // 생성된 몬스터의 stats에 접근가능한 변수
    private Sequence enemyDamageSequence;

    public void OnEnableEnemy()
    {
        enemyInfoCanvasGroup.alpha = 0f;
    }

    // 적 정보 세팅 함수
    public void SetEnemyInfo(int number)
    {
        switch(number)
        {
            case 0:

                enemyNameText.text = null; // 적 이름 변수 클리어

                enemyHPText.text = default; // 적 최대체력 변수 클리어

                enemyHPImage_Back.fillAmount = 1f; // 적 체력 슬라이더를 100%

                enemyInfoCanvasGroup.DOFade(0f, 0f);

                monsterStats = null; // 스텟 변수 클리어

                break;

            case 1:

                enemyInfoCanvasGroup.DOFade(1f, 1.5f);

                enemyNameText.text = monsterStats.monsterData.monsterName; // 적 이름 변수 초기화

                enemyHPText.text = monsterStats.monsterData.maxHp.ToString("#,##0"); // 적 최대체력 변수 초기화

                enemyHPImage_Back.fillAmount = 1f; // 적 체력 슬라이더를 초기화

                break;
        }
    }

    public void OnDisableEnemy()
    {
        DOTween.Kill(this);
        enemyDamageSequence = null;
    }

    // 적 체력 변경 이벤트 발생 시 호출
    private void OnEnemyHealthChanged()
    {
        // 체력 텍스트 업데이트
        enemyHPText.text = monsterStats.CurrentHP.ToString("#,##0");

        enemyDamageSequence.Kill(); // 이전 시퀀스가 실행중이면 종료

        float duration = 0.2f;

        // 닷트윈 색상 변경했다 돌아오기 효과
        enemyDamageSequence = DOTween.Sequence();
        enemyDamageSequence.Append(enemyHPText.DOColor(Color.red, duration));
        enemyDamageSequence.Append(enemyHPText.DOColor(Color.white, duration));
        enemyDamageSequence.Append(enemyHPImage_Front.DOFillAmount(monsterStats.CurrentHP / monsterStats.maxHp, 0f));
        enemyDamageSequence.Append(enemyHPImage_Back.DOFillAmount(monsterStats.CurrentHP / monsterStats.maxHp, 3.0f).SetEase(Ease.OutQuad));
    }
}
