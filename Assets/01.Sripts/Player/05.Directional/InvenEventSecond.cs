using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.UI.Image;

public class InvenEventSecond : MonoBehaviour
{
    public CanvasGroup bgPanel;

    public RectTransform panelA;
    public float panelAOffsetY = 2f;   // 위로 이동할 거리
    public float panelADuration = 1f;   // Fade + 이동 총 시간

    public RectTransform panelB;
    public float panelBOffsetX = 2.5f;  // 왼쪽에서 시작할 거리
    public float panelBDuration = 2f;   // 천천히 이동
    public float panelBFastIn = 1f;     // 빠른 입장 시간

    // 다음 시퀀스 참조 (옵션)
    public InvenEventSecond nextSequence;

    public Sequence PlaySequence(Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        // -------------------------
        // BG 패널 활성화
        bgPanel.gameObject.SetActive(true);

        // -------------------------
        // Panel A
        if (panelA != null)
        {
            panelA.gameObject.SetActive(true);
            Vector2 original = panelA.anchoredPosition;
            panelA.anchoredPosition = new Vector2(original.x, original.y + panelAOffsetY);

            seq.Append(
                panelA
                    .DOAnchorPosY(original.y, panelADuration)
                    .OnComplete(() => panelA.gameObject.SetActive(false))
             );
        }

        // -------------------------
        // 시퀀스 완료 후 다음 시퀀스 실행
        seq.OnComplete(() => onComplete?.Invoke());
        return seq;
    }

    
    public Sequence PanelBSequence()
    {
        var seq = DOTween.Sequence();
        // -------------------------
        // Panel B (빠르게 들어오고 도착할 때 천천히 멈추기)
        if (panelB != null)
        {
            panelB.gameObject.SetActive(true);
            Vector2 original = panelB.anchoredPosition;
            panelB.anchoredPosition = new Vector2(original.x - panelBOffsetX, original.y);

            seq.Append(
                panelB
                    .DOAnchorPosX(original.x, panelBFastIn)
                    .SetEase(Ease.OutCubic)
            );
        }
        return seq;
    }
}