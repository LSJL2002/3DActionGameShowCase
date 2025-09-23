using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InvenEventThird : MonoBehaviour
{
    // 이어받을 panelB
    public RectTransform panelB;
    public float panelBFast = 0.5f;      // panelB가 천천히 시작할 속도
    public float panelBExitDuration = 1f; // panelB가 직진해서 사라질 시간

    // 좌측 패널
    public RectTransform leftPanel1;
    public RectTransform leftPanel2;
    public float leftOffsetX = 2.5f;
    public float leftDuration = 1f;

    // 우측 패널
    public RectTransform rightPanel;
    public float rightOffsetX = 2.5f;
    public float rightDuration = 2f;

    public float stayTime = 0.5f;

    public Sequence PlaySequence()
    {
        Sequence seq = DOTween.Sequence();

        // -------------------------
        // 좌측 패널 이동
        if (leftPanel1 != null)
        {
            leftPanel1.gameObject.SetActive(true);
            Vector2 original = leftPanel1.anchoredPosition;
            leftPanel1.anchoredPosition = original + new Vector2(leftOffsetX, 0f);
            seq.Join(
                        leftPanel1.DOAnchorPosX(original.x, leftDuration)
                                  .SetEase(Ease.OutQuad)
                                  .SetUpdate(true)   // <--- 여기가 핵심
                                  .OnComplete(() =>
                                  {
                                      // 도착 후 좌우로 계속 왕복
                                      leftPanel1.DOAnchorPosY(original.y + 0.2f, 5f)
                                                .SetEase(Ease.InOutSine)
                                                .SetLoops(-1, LoopType.Yoyo)
                                                .SetUpdate(true); // 반복 Yoyo도 unscaled 적용
                                  })
                    );
        }

        if (leftPanel2 != null)
        {
            leftPanel2.gameObject.SetActive(true);
            Vector2 original = leftPanel2.anchoredPosition;
            leftPanel2.anchoredPosition = original + new Vector2(-leftOffsetX, 0f);
            seq.Join(
                        leftPanel2.DOAnchorPosX(original.x, leftDuration)
                                  .SetEase(Ease.OutQuad)
                                  .OnComplete(() =>
                                  {
                                      leftPanel2.DOAnchorPosX(original.x + 0.2f, 5f)
                                                .SetEase(Ease.InOutSine)
                                                .SetLoops(-1, LoopType.Yoyo)
                                                .SetUpdate(true);
                                  })
                    );
        }

        // -------------------------
        // 우측 패널 이동
        if (rightPanel != null)
        {
            rightPanel.gameObject.SetActive(true);
            Vector2 original = rightPanel.anchoredPosition;
            rightPanel.anchoredPosition = original + new Vector2(rightOffsetX, 0f);
            seq.Join(
                        rightPanel.DOAnchorPosX(original.x, rightDuration)
                                  .SetEase(Ease.OutQuad)
                                  .OnComplete(() =>
                                  {
                                      rightPanel.DOAnchorPosX(original.x - 0.05f, 5f)
                                                .SetEase(Ease.InOutSine)
                                                .SetLoops(-1, LoopType.Yoyo)
                                                .SetUpdate(true);
                                  })
                    );
        }

        // 화면에 잠시 머무르기
        seq.AppendInterval(stayTime);

        // -------------------------
        // panelB 직진 후 사라지기
        if (panelB != null)
        {
            seq.Join(
                panelB.DOAnchorPosX(panelB.anchoredPosition.x + 5f, panelBExitDuration)
                        .SetEase(Ease.Linear)
                        .SetUpdate(true)
                        .OnComplete(() => panelB.gameObject.SetActive(false)));
        }

        return seq;
    }

    // panelB만 독립적으로 트윈
    public Sequence PanelBSequence()
    {
        if (panelB == null) return null;

        var seq = DOTween.Sequence();
        panelB.gameObject.SetActive(true);
        seq.Join(panelB.DOAnchorPosX(panelB.anchoredPosition.x + panelBFast, panelBFast)
                        .SetEase(Ease.OutCubic))
                        .SetUpdate(true);
        return seq;
    }
}
