using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

// GameUI의 Skill Part
public partial class GameUI : UIBase
{
    [SerializeField] private CanvasGroup skillInfoCanvasGroup;
    [SerializeField] private Image skill1Image; // 스킬1 이미지
    
    private Tween skill1cooltimeTween; // DOTween 트윈 참조 (중복 Kill을 위해 필요)

    public void OnAwakeSkill()
    {
        skill1Image.fillAmount = 1;
    }

    public void OnEnableSkill()
    {
        skillInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { skillInfoCanvasGroup.DOFade(1f, 1f); });

        UIManager.Instance.playerInput.Player.HeavyAttack.performed += OnCoolTimeHeavyAttack; // 우클릭(헤비어택) 입력 이벤트 구독
    }

    public void OnDisableSkill()
    {
        UIManager.Instance.playerInput.Player.HeavyAttack.performed -= OnCoolTimeHeavyAttack; // 우클릭(헤비어택) 입력 이벤트 구독해제
    }

    public void OnCoolTimeHeavyAttack(InputAction.CallbackContext context)
    {
        skill1cooltimeTween.Kill(); // 중복 방지 (이전 닷트윈 중지)

        float cooltimeDuration = playerStats.SkillCooldown; // 스킬 Max 쿨타임 가져옴 (초단위)

        skill1Image.fillAmount = 1f; // 시작 전 1f로 초기화

        skill1cooltimeTween = skill1Image
            .DOFillAmount(0f, cooltimeDuration)
            .OnComplete(() => { skill1Image.fillAmount = 1f; })
            .SetAutoKill(true).Play();
    }
}
