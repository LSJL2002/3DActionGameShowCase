using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputHoldSystem
{
    private struct HoldData
    {
        public bool IsHolding;    // 버튼 눌려 있는지
        public float StartTime;   // 시작 시간
        public bool HasTriggered; // 홀드 이벤트 발생체크
    }

    private readonly Dictionary<string, HoldData> holdMap = new();
    private readonly float holdThreshold;

    // ============= 이벤트 ============
    public event Action<string> OnHoldStarted;     // 버튼 누르는 즉시 발생
    public event Action<string> OnHoldTriggered;   // 0.5초 이상 눌렀을 때
    public event Action<string> OnHoldCanceled;    // 버튼 뗄 때

    public InputHoldSystem(float threshold = 0.5f)
    {
        holdThreshold = threshold;
    }

    // 버튼 눌렀을 때
    public void Pressed(string button)
    {
        holdMap[button] = new HoldData
        {
            IsHolding = true,
            StartTime = Time.time
        };

        OnHoldStarted?.Invoke(button);
    }

    // 버튼 뗐을 때
    public void Released(string button)
    {
        if (holdMap.TryGetValue(button, out var data))
        {
            if (data.IsHolding)
                OnHoldCanceled?.Invoke(button);

            holdMap[button] = new HoldData(); // 상태 초기화
        }
    }

    // 매 프레임 호출
    public void Update()
    {
        foreach (var key in holdMap.Keys.ToList())
        {
            var data = holdMap[key];

            if (data.IsHolding && Time.time - data.StartTime >= holdThreshold)
            {
                OnHoldTriggered?.Invoke(key);
                holdMap[key] = new HoldData { IsHolding = false };
            }
        }
    }

    // 외부에서 버튼이 현재 홀딩중인지 체크
    public bool IsHolding(string button)
    {
        return holdMap.TryGetValue(button, out var data) && data.IsHolding;
    }
}