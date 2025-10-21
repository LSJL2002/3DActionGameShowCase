using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// GameUI의 Skill Part
public partial class GameUI : UIBase
{
    [SerializeField] private CanvasGroup skillInfoCanvasGroup;

    [SerializeField] private Image evadeImage; // 회피 이미지
    [SerializeField] private TextMeshProUGUI evadeText; // 회피 스택 텍스트

    [SerializeField] private Image skill1Image; // 스킬1 이미지
    [SerializeField] private TextMeshProUGUI skill1Text; // 스킬1 스택 텍스트

    private Sequence evadeCoolTimeSequence = null;
    private Sequence heavyAttackCoolTimeSequence = null;

    public void OnAwakeSkill()
    {
        evadeImage.fillAmount = 1;
        evadeText.text = PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent.ToString();

        skill1Image.fillAmount = 1;
        skill1Text.text = PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent.ToString();
    }

    public void OnEnableSkill()
    {
        skillInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { skillInfoCanvasGroup.DOFade(1f, 1f); });

        PlayerManager.Instance.Input.PlayerActions.HeavyAttack.started += OnCoolTimeHeavyAttack; // 우클릭(헤비어택) 입력 이벤트 구독
        PlayerManager.Instance.Input.PlayerActions.Dodge.started += OnCoolTimeEvade; // 스페이스(회피) 입력 이벤트 구독
    }

    public void OnDisableSkill()
    {
        if (PlayerManager.Instance == null) return;
        PlayerManager.Instance.Input.PlayerActions.HeavyAttack.started -= OnCoolTimeHeavyAttack; // 우클릭(헤비어택) 입력 이벤트 구독해제
        PlayerManager.Instance.Input.PlayerActions.Dodge.started -= OnCoolTimeEvade; // 스페이스(회피) 입력 이벤트 구독해제
    }

    // 회피 쿨타임
    public void OnCoolTimeEvade(InputAction.CallbackContext context)
    {
        if (evadeCoolTimeSequence.IsActive() || PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent == 0 && !PlayerManager.Instance.Ability.IsDodging) return;

        float cooltimeDuration = PlayerManager.Instance.Attr.EvadeBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)

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
    public void OnCoolTimeHeavyAttack(InputAction.CallbackContext context)
    {
        if (heavyAttackCoolTimeSequence.IsActive() || PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent == 0 || PlayerManager.Instance.Ability.IsUsingSkill) return;

        float cooltimeDuration = PlayerManager.Instance.Attr.SkillBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)

        skill1Image.fillAmount = 1f; // 시작 전 1f로 초기화

        heavyAttackCoolTimeSequence = DOTween.Sequence(); // 새로운 시퀀스 생성
        heavyAttackCoolTimeSequence.Append(skill1Image.DOFillAmount(0f, cooltimeDuration)); // 시퀀스에 트윈 추가 (쿨타임 이미지 시각효과)
        heavyAttackCoolTimeSequence.AppendCallback(() => { skill1Image.fillAmount = 1f; });

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        heavyAttackCoolTimeSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => heavyAttackCoolTimeSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작
    }

    // 스킬 UI 스택 업데이트
    public void OnUpdateSkill()
    {
        skill1Text.text = PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent.ToString(); // 스택을 업데이트
        evadeText.text = PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent.ToString(); // 스택을 업데이트
    }
}
