using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerUIElement
{
    public float playerMaxHP;
    public Image hpImageBack;
    public Image hpImageFront;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI nameText;
}

// GameUI의 Player Part
public partial class GameUI : UIBase
{
    [Header("[Player UI Group]")]
    [SerializeField] private PlayerUIElement[] playerUIElements = new PlayerUIElement[3];

    [Header("[Util]")]
    [SerializeField] private CanvasGroup playerInfoCanvasGroup;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image damageEffect;

    private Image activeHPImage_Back;
    private Image activeHPImage_Front;
    private TextMeshProUGUI activeHPText;
    private int playerIndex;
    private Sequence playerDamageSequence;
    private Sequence playerHealSequence;

    public void OnEnablePlayer()
    {
        PlayerCharacter activeCharacter = PlayerManager.Instance.ActiveCharacter;
        UpdatePlayerUI(activeCharacter);

        // 이벤트 구독 / 해제
        PlayerManager.Instance.OnActiveCharacterChanged -= UpdatePlayerUI;
        PlayerManager.Instance.OnActiveCharacterChanged += UpdatePlayerUI;
    }

    public void OnStartPlayer()
    {
        playerInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { playerInfoCanvasGroup.DOFade(1f, 1f); });

        var characterTypes = Enum.GetValues(typeof(CharacterType));

        for (int i = 0; i < playerUIElements.Length; i++)
        {
            // 플레이어 체력UI 기본세팅
            playerUIElements[i].playerMaxHP = PlayerManager.Instance.Characters[i].Ability.Attr.Resource.MaxHealth.Value;
            playerUIElements[i].hpImageBack.fillAmount = 1f;
            playerUIElements[i].hpImageFront.fillAmount = 1f;
            playerUIElements[i].hpText.text = PlayerManager.Instance.Characters[i].Ability.Attr.Resource.CurrentHealth.ToString("#,##0");

            if (i < characterTypes.Length)
            {
                playerUIElements[i].nameText.text = characterTypes.GetValue(i).ToString();
            }
        }
    }

    public void OnDisablePlayer()
    {
        DOTween.Kill(this);
        playerDamageSequence = null;
        playerHealSequence = null;
    }

    // 구독갱신
    public void ResetEventPlayer()
    {
        // 구독해제
        PlayerManager.Instance.Attr.Resource.OnHealthChanged -= PlayerHPDecrease;
        EventsManager.Instance.StopListening(GameEvent.OnStatChanged, PlayerHPDecrease);

        // 구독
        PlayerManager.Instance.Attr.Resource.OnHealthChanged += PlayerHPDecrease;
        EventsManager.Instance.StartListening(GameEvent.OnStatChanged, PlayerHPDecrease);
    }

    // 플레이어UI 업데이트시 호출 (최초, 플레이어 교체시)
    public void UpdatePlayerUI(PlayerCharacter playerCharacter)
    {
        // 현재 캐릭터의 정보를 가져오는 곳 재정의
        playerUIElements[playerIndex].playerMaxHP = PlayerManager.Instance.ActiveCharacter.Attr.Resource.MaxHealth.Value;
        playerIndex = (int)playerCharacter.CharacterType;

        if (playerIndex >= 0 && playerIndex < playerUIElements.Length)
        {
            activeHPImage_Back = playerUIElements[playerIndex].hpImageBack;
            activeHPImage_Front = playerUIElements[playerIndex].hpImageFront;
            activeHPText = playerUIElements[playerIndex].hpText;
        }

        ResetEventPlayer();
    }

    private float PreprocessHPUpdate()
    {
        playerUIElements[playerIndex].playerMaxHP = PlayerManager.Instance.ActiveCharacter.Attr.Resource.MaxHealth.Value;

        playerDamageSequence.Kill();
        playerHealSequence.Kill();

        float currentHealth = PlayerManager.Instance.ActiveCharacter.Attr.Resource.CurrentHealth;

        activeHPText.text = currentHealth.ToString("#,##0");

        if(audioSource != null) 
        audioSource.Stop();

        return currentHealth / playerUIElements[playerIndex].playerMaxHP;
    }

    // 체력 감소
    private void PlayerHPDecrease()
    {
        UpdatePlayerUI(PlayerManager.Instance.ActiveCharacter);

        float playerHPpercentage = PreprocessHPUpdate();
        float duration = 0.2f;

        playerDamageSequence = DOTween.Sequence();
        playerDamageSequence.Append(activeHPText.DOColor(Color.red, duration));
        playerDamageSequence.Append(activeHPText.DOColor(Color.white, duration));
        playerDamageSequence.Append(damageEffect.DOFade(50f/255f, duration));
        playerDamageSequence.Append(damageEffect.DOFade(0f, duration));
        playerDamageSequence.Append(activeHPImage_Front.DOFillAmount(playerHPpercentage, 0f));
        playerDamageSequence.Append(activeHPImage_Back.DOFillAmount(playerHPpercentage, 2.0f).SetEase(Ease.OutQuad));

        // 조건 : ( 0 < 플레이어 체력 < 40% ) 닷트윈 효과(지속)
        if (0 < playerHPpercentage && playerHPpercentage <= 0.4) 
        {
            activeHPImage_Back.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);
            activeHPText.DOColor(Color.red, duration).SetLoops(-1, LoopType.Yoyo);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            activeHPImage_Back.DOKill();
            activeHPText.DOKill();
            activeHPText.DOColor(Color.white, 0f);
            activeHPImage_Front.DOFillAmount(playerHPpercentage, 0f);
            activeHPImage_Back.DOFillAmount(playerHPpercentage, 0f);
        }
    }
}