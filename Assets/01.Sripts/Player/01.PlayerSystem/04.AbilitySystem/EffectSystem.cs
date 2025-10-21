using System;
using System.Collections.Generic;
using UnityEngine;


// ====== 상태 효과 정의 ======
public enum EffectType
{
    Buff,
    Debuff,
    Stun,
    Poison,
    SpeedUp,
    SpeedDown
}

public class Effect
{
    public string Name;
    public EffectType Type;
    public float Duration;          // 남은 시간
    public Action OnStart;          // 효과 시작
    public Action OnUpdate;         // 매 프레임 적용
    public Action OnEnd;            // 효과 종료
}

// 버프/디버프 관리
// 지속시간과 갱신
// 중첩/우선순위 처리
// 이벤트 발생
public class EffectSystem
{
    private List<Effect> activeEffects = new List<Effect>();

    public void ApplyEffect(Effect effect)
    {
        activeEffects.Add(effect);
        effect.OnStart?.Invoke();
    }

    public void RemoveEffect(Effect effect)
    {
        if (activeEffects.Contains(effect))
        {
            effect.OnEnd?.Invoke();
            activeEffects.Remove(effect);
        }
    }

    public void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            effect.Duration -= Time.deltaTime;
            effect.OnUpdate?.Invoke();

            if (effect.Duration <= 0f)
            {
                effect.OnEnd?.Invoke();
                activeEffects.RemoveAt(i);
            }
        }
    }
}
