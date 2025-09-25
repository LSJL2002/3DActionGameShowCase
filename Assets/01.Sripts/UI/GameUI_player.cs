using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

// GameUI의 Player Part
public partial class GameUI : UIBase
{
    [SerializeField] private Image playerHPImage;
    [SerializeField] private Image playerMPImage;

    public TextMeshProUGUI playerHPText;   // UI : 플레이어 체력 텍스트
    public TextMeshProUGUI playerMPText;   // UI : 플레이어 마력 텍스트

    private float playerMaxHP;             // 플레이어 최대 체력

    PlayerStats playerStats;               // 플레이어의 stats에 접근가능한 변수

    public void OnEnablePlayer()
    {
        playerStats = PlayerManager.Instance.Stats;

        // 플레이어 변수 초기화
        playerMaxHP = playerStats.MaxHealth.Value;

        // 플레이어 이미지 fillAmount를 초기화
        playerHPImage.fillAmount = 1f;
        playerMPImage.fillAmount = 1f;

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독
        PlayerManager.Instance.Stats.OnPlayerHealthChanged += OnPlayerHealthChanged;
        PlayerManager.Instance.Stats.OnStatChanged += UpdateStat;
    }

    //public void UpdatePlayer()
    //{
    //    // 플레이어 현재체력
    //    float playerCurrentHP = playerStats.CurrentHealth;

    //    // 플레이어 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
    //    playerHPText.text = Mathf.FloorToInt(playerCurrentHP / playerMaxHP * 100).ToString() + "%";

    //    // 플레이어 체력 슬라이더 업데이트
    //    playerHPImage.fillAmount = playerCurrentHP / playerMaxHP;

    //    // 플레이어 현재마력
    //    float playerCurrentMP = playerStats.CurrentEnergy;

    //    // 플레이어 현재 마력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
    //    playerMPText.text = Mathf.FloorToInt(playerCurrentMP / playerMaxMP * 100).ToString() + "%";

    //    // 플레이어 마력 슬라이더 업데이트
    //    playerMPImage.fillAmount = playerCurrentMP / playerMaxMP;
    //}

    // 플레이어 체력 변경 이벤트 발생 시 호출
    private void OnPlayerHealthChanged()
    {
        float duration = 0.2f;

        // 기존 닷트윈 애니메이션 중지
        playerHPImage.DOKill(); 
        
        // 닷트윈 체력바 fillAmount를 부드럽게 변경
        playerHPImage.DOFillAmount(playerStats.CurrentHealth / playerMaxHP, 1.0f) // 0.5초 동안 부드럽게 변경
                       .SetEase(Ease.OutQuad); // 애니메이션 가속/감속 방식

        // 닷트윈 쉐이크 효과
        playerHPImage.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true);
        playerHPText.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true);

        // 닷트윈 색상 변경했다 돌아오기 효과
        Color originalColor = playerHPText.color;                         // 현재 색상 값을 저장
        Sequence mySequence = DOTween.Sequence();                         // 새로운 시퀀스 생성
        mySequence.Append(playerHPText.DOColor(Color.red, duration));     // 시퀀스에 첫 번째 트윈 추가 (빨간색으로 변경)
        mySequence.Append(playerHPText.DOColor(originalColor, duration)); // 시퀀스에 두 번째 트윈 추가 (원래 색상으로 돌아오기)

        // 체력 텍스트도 업데이트
        playerHPText.text = Mathf.FloorToInt(playerStats.CurrentHealth / playerMaxHP * 100).ToString() + "%";
    }

    // 플레이어 스탯이 변화했을때 호출 할 함수
    public void UpdateStat(StatType statType)
    {
        // 플레이어 맥스체력,마력을 업데이트
        playerMaxHP = playerStats.MaxHealth.Value;
    }
}
