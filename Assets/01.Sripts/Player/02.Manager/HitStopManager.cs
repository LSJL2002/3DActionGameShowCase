using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    private bool isHitStopping = false;

    /// <summary>
    /// 타임스케일을 잠깐 줄였다가 되돌리는 함수
    /// </summary>
    /// <param name="duration">타임스케일이 지속되는 실제 시간(초, Realtime 기준)</param>
    /// <param name="timeScale">적용할 타임스케일 값 (0 = 완전 정지, 0.2 = 슬로우)</param>
    public void DoHitStop(float duration = 0.2f, float timeScale = 0.2f)
    {
        if (!isHitStopping) // 중복 실행 방지
            StartCoroutine(HitStopRoutine(duration, timeScale));
    }

    private IEnumerator HitStopRoutine(float duration, float timeScale)
    {
        isHitStopping = true;
        float originalScale = Time.timeScale;

        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration); // 실제 시간 기준으로 기다림
        Time.timeScale = originalScale;

        isHitStopping = false;
    }
}