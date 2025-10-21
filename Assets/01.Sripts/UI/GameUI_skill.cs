using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// GameUI의 Skill Part
public partial class GameUI : UIBase
{
    [Header("[Skill]")]
    [SerializeField] private CanvasGroup skillInfoCanvasGroup;
    [SerializeField] private Image evadeImage; // 회피 이미지
    [SerializeField] private TextMeshProUGUI evadeText; // 회피 스택 텍스트
    [SerializeField] private Image skill1Image; // 스킬1 이미지
    [SerializeField] private TextMeshProUGUI skill1Text; // 스킬1 스택 텍스트

    private Sequence evadeCoolTimeSequence = null;
    private Sequence skillCoolTimeSequence = null;

    public void OnEnableSkill()
    {
        skillInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { skillInfoCanvasGroup.DOFade(1f, 1f); });

        PlayerCharacter activeCharacter = PlayerManager.Instance.ActiveCharacter;
        UpdateSkillUI(activeCharacter);

        // 이벤트 구독 / 해제
        PlayerManager.Instance.OnActiveCharacterChanged -= UpdateSkillUI;
        PlayerManager.Instance.OnActiveCharacterChanged += UpdateSkillUI;
    }

    public void OnDisableSkill()
    {
        DOTween.Kill(this);
        evadeCoolTimeSequence = null;
        skillCoolTimeSequence = null;
    }

    // 구독갱신
    public void ResetEventSkill()
    {
        // 구독해제
        PlayerManager.Instance.Attr.EvadeBuffer.OnBufferChanged -= OnCoolTimeEvade;
        PlayerManager.Instance.Attr.SkillBuffer.OnBufferChanged -= OnCoolTimeSkill;

        // 구독
        PlayerManager.Instance.Attr.EvadeBuffer.OnBufferChanged += OnCoolTimeEvade;
        PlayerManager.Instance.Attr.SkillBuffer.OnBufferChanged += OnCoolTimeSkill;
    }

    // SkillUI 업데이트시 호출 (최초, 플레이어 교체시)
    public void UpdateSkillUI(PlayerCharacter playerCharacter)
    {
        evadeCoolTimeSequence.Complete();
        skillCoolTimeSequence.Complete();

        evadeImage.fillAmount = 1;
        evadeText.text = PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent.ToString();

        skill1Image.fillAmount = 1;
        skill1Text.text = PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent.ToString();

        ResetEventSkill();
    }

    // 회피 쿨타임
    public void OnCoolTimeEvade()
    {
        float cooltimeDuration = PlayerManager.Instance.Attr.EvadeBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)
        evadeText.text = PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent.ToString(); // 스택을 업데이트

        if (PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent == PlayerManager.Instance.Attr.EvadeBuffer.BufferMax)
            return;

        evadeImage.fillAmount = 1f; // 시작 전 1f로 초기화

        evadeCoolTimeSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
        evadeCoolTimeSequence.Append(evadeImage.DOFillAmount(0f, cooltimeDuration)); // 시퀀스에 트윈 추가 (쿨타임 이미지 시각효과)
        evadeCoolTimeSequence.AppendCallback(() => { evadeImage.fillAmount = 1f; });

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        evadeCoolTimeSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => evadeCoolTimeSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작
    }

    // 스킬1 쿨타임
    public void OnCoolTimeSkill()
    {
        float cooltimeDuration = PlayerManager.Instance.Attr.SkillBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)
        skill1Text.text = PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent.ToString(); // 스택을 업데이트

        if (PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent == PlayerManager.Instance.Attr.SkillBuffer.BufferMax)
            return;

        skill1Image.fillAmount = 1f; // 시작 전 1f로 초기화

        skillCoolTimeSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
        skillCoolTimeSequence.Append(skill1Image.DOFillAmount(0f, cooltimeDuration)); // 시퀀스에 트윈 추가 (쿨타임 이미지 시각효과)
        skillCoolTimeSequence.AppendCallback(() => { skill1Image.fillAmount = 1f; });

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        skillCoolTimeSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => skillCoolTimeSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작
    }
}
