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
    public float panelBOffsetX = 2.5f;       // 왼쪽에서 시작
    public float panelBFastIn = 1f;          // 빠른 입장 시간
    public float panelBSlowDuration = 1f;    // 천천히 이동
    public float panelBExitDistance = 0.5f;    // 퇴장 거리
    public float panelBExitDuration = 1f;    // 퇴장 시간
    public float panelBStayTime = 0.5f;        // 화면 머무는 시간

    private Vector2 panelBOriginalPos;   // 원래 자리 저장용
    public Vector2 PanelBOriginalPos => panelBOriginalPos; // 읽기 전용 공개 프로퍼티


    void Awake()   // 게임 시작 시 한 번만 실행
    {
        if (panelB != null)
            panelBOriginalPos = panelB.anchoredPosition;
    }

    public Sequence PlaySequence(Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        // -------------------------
        // BG 패널 활성화
        if (bgPanel != null) bgPanel.gameObject.SetActive(true);

        // -------------------------
        // Panel A
        if (panelA != null)
        {
            panelA.gameObject.SetActive(true);
            Vector2 original = panelA.anchoredPosition;
            panelA.anchoredPosition = new Vector2(original.x, original.y + panelAOffsetY);

            seq.Append(panelA
                            .DOAnchorPosY(original.y, panelADuration)
                            .SetUpdate(true)
                            .OnComplete(() => panelA.gameObject.SetActive(false))
                        );
        }

        // -------------------------
        // 시퀀스 완료 후 다음 시퀀스 실행
        seq.OnComplete(() => onComplete?.Invoke());
        return seq;
    }

    // -------------------------
    // Panel B 독립적 제어
    public Sequence PanelBSequence()
    {
        var seq = DOTween.Sequence();
        if (panelB == null) return seq;

        panelB.gameObject.SetActive(true);

        // 1. 빠른 등장
        panelB.anchoredPosition = new Vector2(panelBOriginalPos.x - panelBOffsetX, panelBOriginalPos.y);
        seq.Append(panelB
            .DOAnchorPosX(panelBOriginalPos.x, panelBFastIn)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
        );

        // 2. 천천히 이동
        seq.Append(panelB
            .DOAnchorPosX(panelBOriginalPos.x + panelBSlowDuration, panelBSlowDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
        );

        // 3. 화면에 잠시 머무르기
        seq.AppendInterval(panelBStayTime);

        // 4. 퇴장 (3차 시점에서 호출)
        seq.Append(panelB
            .DOAnchorPosX(panelBOriginalPos.x + panelBExitDistance, panelBExitDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() => panelB.gameObject.SetActive(false))
        );

        return seq;
    }
}