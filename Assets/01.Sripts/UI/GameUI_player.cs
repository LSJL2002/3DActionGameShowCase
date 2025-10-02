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

    Sequence playerDamageSequence;
    Sequence playerHealSequence;

    float previousHP; // 이전 체력 값을 저장하는 변수
    float currentHP; // 현재 체력 값을 저장하는 변수

    public void OnAwakePlayer()
    {
        playerStats = PlayerManager.Instance.Stats;

        // 플레이어 변수 초기화
        playerMaxHP = playerStats.MaxHealth.Value;
        previousHP = playerStats.MaxHealth.Value;

        // 플레이어 이미지 fillAmount를 초기화
        playerHPImage.fillAmount = 1f;
        playerMPImage.fillAmount = 1f;

        playerHPText.text = playerStats.CurrentHealth.ToString("#,##0");
    }

    public void OnEnablePlayer()
    {
        playerInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { playerInfoCanvasGroup.DOFade(1f, 1f); });

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독해제 (중복구독 방지)
        PlayerManager.Instance.Stats.OnPlayerHealthChanged -= OnPlayerHealthChanged;
        PlayerManager.Instance.Stats.OnStatChanged -= UpdateStat;

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독
        PlayerManager.Instance.Stats.OnPlayerHealthChanged += OnPlayerHealthChanged;
        PlayerManager.Instance.Stats.OnStatChanged += UpdateStat;
    }

    public void OnDisablePlayer()
    {
        DOTween.Kill(this);
        playerDamageSequence = null;
        playerHealSequence = null;
    }

    // 플레이어 체력 변경 이벤트 발생 시 호출
    private async void OnPlayerHealthChanged()
    {
        playerDamageSequence.Kill(); // 기존 시퀀스가 있다면 종료
        playerHealSequence.Kill(); // 기존 시퀀스가 있다면 종료

        // 체력 텍스트 업데이트
        playerHPText.text = playerStats.CurrentHealth.ToString("#,##0");
        float playerHPpercentage = playerStats.CurrentHealth / playerMaxHP;

        // 플레이어 체력이 40% 이하가 되면 닷트윈 효과(지속)
        if (playerHPpercentage <= 0.4)
        {
            playerHPImage.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);
            playerHPText.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);
            
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        // 체력이 감소
        else if (playerStats.CurrentHealth < previousHP)
        {
            audioSource.Stop();

            await UIManager.Instance.Show<DamageEffectUI>();

            // 체력 텍스트 붉게 변했다가 돌아오기
            Color originalColor = playerHPText.color;
            playerHPText.DOColor(Color.red, duration).OnComplete(() => { playerHPText.DOColor(originalColor, duration); });

            if (playerDamageSequence == null)
            {
                playerDamageSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
                playerDamageSequence.Append(playerHPImage.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true)); // 시퀀스에 트윈 추가 (체력바 : 흔들림)
                playerDamageSequence.Append(playerHPText.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true)); // 시퀀스에 트윈 추가 (체력텍스트 : 흔들림)
                playerDamageSequence.Append(playerHPImage.DOFillAmount(playerStats.CurrentHealth / playerMaxHP, 1.0f).SetEase(Ease.OutQuad)); // 시퀀스에 트윈 추가 (체력바 : 부드럽게 감소)
            }
        }
        // 체력이 증가
        else if (playerStats.CurrentHealth > previousHP)
        {
            audioSource.Stop();
            // 체력 텍스트 푸르게 변했다가 돌아오기
            Color originalColor = playerHPText.color;
            playerHPText.DOColor(Color.green, duration).OnComplete(() => { playerHPText.DOColor(originalColor, duration); });

            playerHealSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
            playerHealSequence.Append(playerHPImage.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true)); // 시퀀스에 트윈 추가 (체력바 : 흔들림)
            playerHealSequence.Append(playerHPText.rectTransform.DOShakePosition(duration, 10, 10, 90, true, true)); // 시퀀스에 트윈 추가 (체력텍스트 : 흔들림)
            playerHealSequence.Append(playerHPImage.DOFillAmount(playerStats.CurrentHealth / playerMaxHP, 1.0f).SetEase(Ease.OutQuad)); // 시퀀스에 트윈 추가 (체력바 : 부드럽게 감소)
        }

        previousHP = playerStats.CurrentHealth;
    }

    // 플레이어 스탯이 변화했을때 호출 할 함수
    public void UpdateStat(StatType statType)
    {
        // 플레이어 맥스체력,마력을 업데이트
        playerMaxHP = playerStats.MaxHealth.Value;
    }
}
