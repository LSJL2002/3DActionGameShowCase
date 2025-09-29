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
    [SerializeField] private CanvasGroup playerInfoCanvasGroup;

    public TextMeshProUGUI playerHPText;   // UI : 플레이어 체력 텍스트

    private float playerMaxHP;             // 플레이어 최대 체력

    PlayerStats playerStats;               // 플레이어의 stats에 접근가능한 변수

    [SerializeField] private AudioSource audioSource;

    float duration = 0.2f; // 닷트윈 효과들에서 사용할 시간

    public void OnAwakePlayer()
    {
        playerStats = PlayerManager.Instance.Stats;

        // 플레이어 변수 초기화
        playerMaxHP = playerStats.MaxHealth.Value;

        // 플레이어 이미지 fillAmount를 초기화
        playerHPImage.fillAmount = 1f;
        playerMPImage.fillAmount = 1f;
    }

    public void OnEnablePlayer()
    {
        playerInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { playerInfoCanvasGroup.DOFade(1f, 1f); });

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독
        PlayerManager.Instance.Stats.OnPlayerHealthChanged += OnPlayerHealthChanged;
        PlayerManager.Instance.Stats.OnStatChanged += UpdateStat;
    }

    public void OnDisablePlayer()
    {
        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독 해제
        PlayerManager.Instance.Stats.OnPlayerHealthChanged -= OnPlayerHealthChanged;
        PlayerManager.Instance.Stats.OnStatChanged -= UpdateStat;
    }

    // 플레이어 체력 변경 이벤트 발생 시 호출
    private async void OnPlayerHealthChanged()
    {
        await UIManager.Instance.Show<DamageEffectUI>();

        Debug.Log(playerStats.CurrentHealth);

        // 체력 텍스트 업데이트
        playerHPText.text = Mathf.FloorToInt(playerStats.CurrentHealth / playerMaxHP * 100).ToString() + "%";
        float playerHPpercentage = playerStats.CurrentHealth / playerMaxHP;

        // 플레이어 체력이 40% 이하가 되면 닷트윈 효과(지속)
        if (playerHPpercentage <= 0.4)
        {
            playerHPImage.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);
            playerHPText.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();

            // 체력 텍스트 붉게 변했다가 돌아오기
            Color originalColor = playerHPText.color;
            playerHPText.DOColor(Color.red, duration).OnComplete(() => { playerHPText.DOColor(originalColor, duration); });
        }

        Sequence mySequence = DOTween.Sequence(); // 새로운 시퀀스 생성
        mySequence.Append(playerHPImage.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true)); // 시퀀스에 트윈 추가 (체력바 : 흔들림)
        mySequence.Append(playerHPText.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true)); // 시퀀스에 트윈 추가 (체력텍스트 : 흔들림)
        mySequence.Append(playerHPImage.DOFillAmount(playerStats.CurrentHealth / playerMaxHP, 1.0f).SetEase(Ease.OutQuad)); // 시퀀스에 트윈 추가 (체력바 : 부드럽게 감소)
    }

    // 플레이어 스탯이 변화했을때 호출 할 함수
    public void UpdateStat(StatType statType)
    {
        // 플레이어 맥스체력,마력을 업데이트
        playerMaxHP = playerStats.MaxHealth.Value;
    }
}
