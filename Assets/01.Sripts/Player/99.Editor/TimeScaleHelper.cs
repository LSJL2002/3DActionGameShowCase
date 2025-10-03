using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class TimeScaleHelper
{
    // ========================
    // DoTween 헬퍼
    // ========================
    // unscaled = true면 Time.timeScale 영향 받지 않음
    public static Tweener DoMoveX(Transform target, float x, float duration, bool unscaled = false)
    {
        return target.DOMoveX(x, duration).SetUpdate(unscaled);
    }

    public static Tweener DoMoveY(Transform target, float y, float duration, bool unscaled = false)
    {
        return target.DOMoveY(y, duration).SetUpdate(unscaled);
    }

    public static Tweener DoMove(Transform target, Vector3 pos, float duration, bool unscaled = false)
    {
        return target.DOMove(pos, duration).SetUpdate(unscaled);
    }

    public static Tweener DoScale(Transform target, Vector3 scale, float duration, bool unscaled = false)
    {
        return target.DOScale(scale, duration).SetUpdate(unscaled);
    }

    public static Tweener DoRotate(Transform target, Vector3 euler, float duration, bool unscaled = false)
    {
        return target.DORotate(euler, duration).SetUpdate(unscaled);
    }

    // ========================
    //  코루틴 헬퍼
    // ========================
    public static IEnumerator Wait(float seconds, bool unscaled = false)
    {
        if (unscaled)
            yield return new WaitForSecondsRealtime(seconds);
        else
            yield return new WaitForSeconds(seconds);
    }

    // ========================
    //  Animator 헬퍼
    // ========================
    // UI 캐릭터용 Animator를 unscaled로 설정
    public static void SetAnimatorUnscaled(Animator animator)
    {
        if (animator != null)
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
}