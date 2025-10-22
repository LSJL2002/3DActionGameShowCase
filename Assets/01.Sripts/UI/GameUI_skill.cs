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

        float evadeCooltimeDuration = PlayerManager.Instance.Attr.EvadeBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)
        float skillCooltimeDuration = PlayerManager.Instance.Attr.SkillBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)

        // 회피 쿨타임 시퀀스 초기 생성
        evadeCoolTimeSequence = DOTween.Sequence();
        evadeCoolTimeSequence.Append(evadeImage.DOFillAmount(0f, evadeCooltimeDuration));
        evadeCoolTimeSequence.AppendCallback(() => { evadeImage.fillAmount = 1f; });
        evadeCoolTimeSequence.SetAutoKill(false);

        // 스킬 쿨타임 시퀀스 초기 생성
        skillCoolTimeSequence = DOTween.Sequence();
        skillCoolTimeSequence.Append(skill1Image.DOFillAmount(0f, skillCooltimeDuration));
        skillCoolTimeSequence.AppendCallback(() => { skill1Image.fillAmount = 1f; });
        skillCoolTimeSequence.SetAutoKill(false);

        // 최초엔 정지
        evadeCoolTimeSequence.Pause();
        skillCoolTimeSequence.Pause();
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
        if (evadeCoolTimeSequence.IsActive())
            evadeCoolTimeSequence.Rewind(); // 처음으로 되감기 + Pause
        if (skillCoolTimeSequence.IsActive())
            skillCoolTimeSequence.Rewind(); // 처음으로 되감기 + Pause

        evadeImage.fillAmount = 1;
        evadeText.text = PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent.ToString();

        skill1Image.fillAmount = 1;
        skill1Text.text = PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent.ToString();

        ResetEventSkill();
    }

    // 회피 쿨타임
    public void OnCoolTimeEvade()
    {
        BufferModule evadeBuffer = PlayerManager.Instance.Attr.EvadeBuffer;
        evadeText.text = evadeBuffer.BufferCurrent.ToString(); // 스택을 업데이트

        if (evadeBuffer.BufferCurrent == evadeBuffer.BufferMax || evadeCoolTimeSequence.IsPlaying())
            return;

        evadeImage.fillAmount = 1f; // 시작 전 1f로 초기화
        evadeCoolTimeSequence.Restart();
    }

    // 스킬 쿨타임
    public void OnCoolTimeSkill()
    {
        BufferModule skillBuffer = PlayerManager.Instance.Attr.EvadeBuffer;
        skill1Text.text = skillBuffer.BufferCurrent.ToString(); // 스택을 업데이트

        if (skillBuffer.BufferCurrent == skillBuffer.BufferMax || skillCoolTimeSequence.IsPlaying())
            return;

        skill1Image.fillAmount = 1f; // 시작 전 1f로 초기화
        skillCoolTimeSequence.Restart();
    }
}
