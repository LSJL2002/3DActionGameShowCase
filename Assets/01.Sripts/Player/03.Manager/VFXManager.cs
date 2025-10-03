using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject playerModel; // 아바타 루트
    [SerializeField] private float blinkInterval = 1f;
    [SerializeField] private int blinkCount = 4;

    private Tween blinkTween;


    public void StartDash()
    {
        if (playerModel == null) return;

        blinkTween?.Kill();

        // 껐다 켜는 깜빡임
        int counter = 0;
        blinkTween = DOVirtual.DelayedCall(blinkInterval, null)
            .SetLoops(blinkCount * 2, LoopType.Yoyo)
            .OnStepComplete(() =>
            {
                playerModel.SetActive(!playerModel.activeSelf);
                counter++;
            })
            .OnKill(() => playerModel.SetActive(true)); // 종료 시 켜기
    }

    public void StopDash()
    {
        blinkTween?.Kill();
        playerModel.SetActive(true); // 확실히 켜기
    }
}