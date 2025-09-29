using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    private BaseMonster baseMonster;

    private void Awake()
    {
        baseMonster = GetComponentInParent<BaseMonster>();
    }

    // AnimationEvent callback
    public void OnAttackAnimationComplete()
    {
        if (baseMonster != null)
            baseMonster.OnAttackAnimationComplete();
    }

    public void OnAttackHitEvent()
    {
        if (baseMonster != null)
            baseMonster.OnAttackHitEvent();
    }

    public void OnDeathAnimationComplete()
    {
        if (baseMonster != null)
            baseMonster.OnDeathAnimationComplete();
    }
}
