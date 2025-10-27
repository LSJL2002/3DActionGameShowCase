using System;
using System.Collections.Generic;
using UnityEngine;


// ==================== 스킬/회피 버퍼 관리 (쿨타임 포함) ===================
public class BufferModule
{
    private readonly Queue<float> recoveryTimes = new();
    private float lastTime;

    public int BufferMax { get; private set; }
    public int BufferCurrent { get; private set; }
    public float Cooldown { get; private set; }

    public event Action OnBufferChanged;

    public BufferModule(int max, float cooldown, int? startValue = null)
    {
        BufferMax = max;
        Cooldown = cooldown;
        BufferCurrent = Mathf.Clamp(startValue ?? 0, 0, BufferMax);
        lastTime = 0f;

        // 자동 회복 모드: startValue < max이면 부족한 만큼 복구 예약 생성
        if (BufferCurrent < BufferMax)
        {
            // 첫 복구는 Time.time + Cooldown, 그 다음은 +Cooldown 간격으로 쌓음
            float baseTime = Time.time;
            for (int i = 0; i < (BufferMax - BufferCurrent); i++)
            {
                float recoverAt = baseTime + Cooldown * (i + 2);
                recoveryTimes.Enqueue(recoverAt);
                lastTime = recoverAt;
            }
        }
    }

    public bool Use()
    {
        if (BufferCurrent <= 0) return false;
        BufferCurrent--;
        OnBufferChanged?.Invoke();
        EnqueueCooldown();
        return true;
    }

    private void EnqueueCooldown()
    {
        float nextRecover = Mathf.Max(Time.time + Cooldown, lastTime + Cooldown);
        recoveryTimes.Enqueue(nextRecover);
        lastTime = nextRecover;
    }

    public void Update()
    {
        bool changed = false;
        while (recoveryTimes.Count > 0 && recoveryTimes.Peek() <= Time.time)
        {
            recoveryTimes.Dequeue();
            if (BufferCurrent < BufferMax)
            {
                BufferCurrent++;
                changed = true;
            }
        }

        if (changed)
            OnBufferChanged?.Invoke();
    }
}