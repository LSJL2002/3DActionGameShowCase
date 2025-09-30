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
    [SerializeField] private Image enemyHPImage;
    [SerializeField] public TextMeshProUGUI enemyNameText;  // UI : 적 이름 텍스트
    [SerializeField] public TextMeshProUGUI enemyHPText; // UI : 적 체력 텍스트
    [SerializeField] private CanvasGroup enemyInfoCanvasGroup;
    [SerializeField] private Sprite enemyDefaultIcon;
    [SerializeField] private Sprite enemyClearIcon;
    private MonsterStatHandler monsterStats;       // 생성된 몬스터의 stats에 접근가능한 변수
    
    public void OnEnableEnemy()
    {
        enemyInfoCanvasGroup.DOFade(0f, 0f);
    }

    // 적 정보 세팅 함수
    public void SetEnemyInfo(int number)
    {
        switch(number)
        {
            case 0:

                enemyNameText.text = null; // 적 이름 변수 클리어

                enemyHPText.text = default; // 적 최대체력 변수 클리어

                enemyHPImage.fillAmount = 1f; // 적 체력 슬라이더를 100%

                enemyInfoCanvasGroup.DOFade(0f, 0f);

                monsterStats = null; // 스텟 변수 클리어

                break;

            case 1:

                enemyInfoCanvasGroup.DOFade(1f, 1.5f);

                enemyNameText.text = monsterStats.monsterData.monsterName; // 적 이름 변수 초기화

                enemyHPText.text = monsterStats.monsterData.maxHp.ToString("#,##0"); // 적 최대체력 변수 초기화

                enemyHPImage.fillAmount = 1f; // 적 체력 슬라이더를 초기화

                break;
        }
    }

    // 적 체력 변경 이벤트 발생 시 호출
    private void OnEnemyHealthChanged()
    {
        float duration = 0.2f;

        // 닷트윈 체력바 fillAmount를 부드럽게 변경
        enemyHPImage.DOFillAmount(monsterStats.CurrentHP / monsterStats.maxHp, 1.0f) // 0.5초 동안 부드럽게 변경
                       .SetEase(Ease.OutQuad); // 애니메이션 가속/감속 방식

        // 닷트윈 쉐이크 효과
        enemyHPImage.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true);
        enemyHPText.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true);

        // 닷트윈 색상 변경했다 돌아오기 효과
        Color originalColor = enemyHPText.color;                         // 현재 색상 값을 저장
        Sequence mySequence = DOTween.Sequence();                        // 새로운 시퀀스 생성
        mySequence.Append(enemyHPText.DOColor(Color.red, duration));     // 시퀀스에 첫 번째 트윈 추가 (빨간색으로 변경)
        mySequence.Append(enemyHPText.DOColor(originalColor, duration)); // 시퀀스에 두 번째 트윈 추가 (원래 색상으로 돌아오기)

        // 체력 텍스트도 업데이트
        enemyHPText.text = monsterStats.CurrentHP.ToString("#,##0");
    }
}
