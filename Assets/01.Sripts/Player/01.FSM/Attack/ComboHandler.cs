using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComboHandler
{
    private int currentIndex = -1;          // 현재 콤보 인덱스 (-1 = 비활성)
    private bool queued;                    // 다음 콤보 예약 여부
    private float lastInputTime;
    private readonly List<AttackInfoData> attacks;
    private Animator animator;


    public ComboHandler(List<AttackInfoData> attacks, Animator animator)
    {
        this.attacks = attacks;
        this.animator = animator;
    }


    /// <summary>
    /// 공격 입력 등록
    /// </summary>
    public void RegisterInput()
    {
        lastInputTime = Time.time;

        if (currentIndex < 0)
        {
            SetCombo(0);
            return;
        }

        // elapsed / animationLength → normalizedTime
        float normalizedTime = GetNormalizedTime();
        var step = attacks[currentIndex];

        bool withinTiming = normalizedTime >= step.ComboTimingStart && normalizedTime <= step.ComboTimingEnd;
        bool withinBuffer = Time.time - lastInputTime <= step.InputBufferTime;

        if (withinTiming || withinBuffer)
            queued = true;
    }

    /// <summary>
    /// 매 프레임 업데이트
    /// </summary>
    /// <returns>현재 AttackInfoData (없으면 null)</returns>
    public AttackInfoData Update()
    {
        if (currentIndex < 0 || attacks.Count == 0) return null;

        float normalizedTime = GetNormalizedTime();
        var step = attacks[currentIndex];

        // 다음 콤보 예약 조건
        if (queued && normalizedTime >= step.ComboTimingStart)
        {
            int next = currentIndex + 1;
            if (next < attacks.Count)
                SetCombo(next);
            else
                Reset();
        }
        // 애니메이션 끝났으면 리셋
        else if (normalizedTime >= 1f)
        {
            Reset();
        }

        return currentIndex >= 0 ? attacks[currentIndex] : null;
    }

    private float GetNormalizedTime()
    {
        if (animator == null) return 0f;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo next = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && next.IsTag("Attack"))
            return next.normalizedTime;
        else if (!animator.IsInTransition(0) && state.IsTag("Attack"))
            return state.normalizedTime;
        else
            return 0f;
    }

    private void SetCombo(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, attacks.Count - 1);
        queued = false;

        // 애니메이션 CrossFade
        animator.CrossFade(attacks[currentIndex].AttackName, 0.05f);
    }

    private void Reset()
    {
        currentIndex = -1;
        queued = false;
    }

    public bool IsActive => currentIndex >= 0;
}