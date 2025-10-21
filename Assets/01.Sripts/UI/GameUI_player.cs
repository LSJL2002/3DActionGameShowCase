using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// GameUI의 Player Part
public partial class GameUI : UIBase
{
    [SerializeField] private Image playerHPImage_Back;
    [SerializeField] private Image playerHPImage_Front;
    [SerializeField] private CanvasGroup playerInfoCanvasGroup;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] public TextMeshProUGUI playerHPText; // UI : 플레이어 체력 텍스트

    private float playerMaxHP; // 플레이어 최대 체력
    private float previousHP; // 이전 체력 값을 저장하는 변수

    private PlayerAttribute playerStats; // 플레이어의 stats에 접근가능한 변수
    private Sequence playerDamageSequence;
    private Sequence playerHealSequence;

    public void OnAwakePlayer()
    {
        playerStats = PlayerManager.Instance.Attr;

        // 플레이어 변수 초기화
        playerMaxHP = playerStats.Resource.MaxHealth.Value;
        previousHP = playerStats.Resource.MaxHealth.Value;

        // 플레이어 이미지 fillAmount를 초기화
        playerHPImage_Back.fillAmount = 1f;

        playerHPText.text = playerStats.Resource.CurrentHealth.ToString("#,##0");
    }

    public void OnEnablePlayer()
    {
        playerInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { playerInfoCanvasGroup.DOFade(1f, 1f); });

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독해제 (중복구독 방지)
        PlayerManager.Instance.Attr.Resource.OnHealthChanged -= OnPlayerHealthChanged;
        EventsManager.Instance.StopListening(GameEvent.OnStatChanged, UpdateStat);

        // 플레이어 체력,마력 증감 이벤트, 스탯증감 이벤트 구독
        PlayerManager.Instance.Attr.Resource.OnHealthChanged += OnPlayerHealthChanged;
        EventsManager.Instance.StartListening(GameEvent.OnStatChanged, UpdateStat);
    }

    public void OnDisablePlayer()
    {
        DOTween.Kill(this);
        playerDamageSequence = null;
        playerHealSequence = null;
    }

    // 플레이어 체력 변경 이벤트 발생 시 호출
    private void OnPlayerHealthChanged()
    {
        playerDamageSequence.Kill(); // 기존 시퀀스가 있다면 종료
        playerHealSequence.Kill(); // 기존 시퀀스가 있다면 종료

        float duration = 0.2f;

        // 체력 텍스트 업데이트
        playerHPText.text = playerStats.Resource.CurrentHealth.ToString("#,##0");
        float playerHPpercentage = playerStats.Resource.CurrentHealth / playerMaxHP;

        // 체력이 감소
        if (playerStats.Resource.CurrentHealth < previousHP)
        {
            audioSource.Stop();

            playerDamageSequence = DOTween.Sequence();
            playerDamageSequence.Append(playerHPText.DOColor(Color.red, duration));
            playerDamageSequence.Append(playerHPText.DOColor(Color.white, duration));
            playerDamageSequence.Append(playerHPImage_Front.DOFillAmount(playerStats.Resource.CurrentHealth / playerMaxHP, 0f));
            playerDamageSequence.Append(playerHPImage_Back.DOFillAmount(playerStats.Resource.CurrentHealth / playerMaxHP, 3.0f).SetEase(Ease.OutQuad));

            // 플레이어 체력이 40% 이하가 되면 닷트윈 효과(지속)
            if (playerHPpercentage <= 0.4)
            {
                playerHPImage_Back.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);
                playerHPText.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }

        // 체력이 증가
        else if (playerStats.Resource.CurrentHealth > previousHP)
        {
            audioSource.Stop();

            playerHealSequence = DOTween.Sequence();
            playerHealSequence.Append(playerHPText.DOColor(Color.green, duration));
            playerHealSequence.Append(playerHPText.DOColor(Color.white, duration));
            playerHealSequence.Append(playerHPImage_Front.DOFillAmount(playerStats.Resource.CurrentHealth / playerMaxHP, 0f));
            playerHealSequence.Append(playerHPImage_Back.DOFillAmount(playerStats.Resource.CurrentHealth / playerMaxHP, 3.0f).SetEase(Ease.OutQuad));
        }

        previousHP = playerStats.Resource.CurrentHealth;
    }

    // 플레이어 스탯이 변화했을때 호출 할 함수
    public void UpdateStat()
    {
        // 플레이어 맥스체력,마력을 업데이트
        playerMaxHP = playerStats.Resource.MaxHealth.Value;
    }
}
