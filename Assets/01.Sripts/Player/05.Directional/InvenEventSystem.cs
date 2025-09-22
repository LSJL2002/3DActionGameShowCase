using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InvenEventSystem : MonoBehaviour
{
    public InvenEventFirst firstSequence;
    public InvenEventSecond secondSequence;
    public InvenEventThird thirdSequence;

    void Start()
    {
        // 2차 판넬 기본 꺼두기
        if (secondSequence.bgPanel != null) secondSequence.bgPanel.gameObject.SetActive(false);
        if (secondSequence.panelA != null) secondSequence.panelA.gameObject.SetActive(false);
        if (secondSequence.panelB != null) secondSequence.panelB.gameObject.SetActive(false);
        // 3차 좌/우 패널 기본 꺼두기
        // 3차 패널 기본 꺼두기
        if (thirdSequence.leftPanel1 != null)
            thirdSequence.leftPanel1.gameObject.SetActive(false);

        if (thirdSequence.leftPanel2 != null)
            thirdSequence.leftPanel2.gameObject.SetActive(false);

        if (thirdSequence.rightPanel != null)
            thirdSequence.rightPanel.gameObject.SetActive(false);

        // 1차 시퀀스 실행
        var firstSeq = firstSequence.PlaySequence(() =>
        {
            // 1차 끝나면 2차 실행
            var secondSeq = DOTween.Sequence();
            secondSeq.Join(secondSequence.PlaySequence());
            secondSeq.Join(secondSequence.PanelBSequence());

            secondSeq.OnComplete(() =>
            {
                // 2차 panelB 위치 이어받아 3차 panelB 시작 위치 설정
                if (thirdSequence.panelB != null && secondSequence.panelB != null)
                {
                    thirdSequence.panelB.anchoredPosition = secondSequence.panelB.anchoredPosition;
                    thirdSequence.panelB.gameObject.SetActive(true);
                }

                // 3차 시퀀스 실행
                var thirdSeq = DOTween.Sequence();
                thirdSeq.Join(thirdSequence.PlaySequence());
                thirdSeq.Join(thirdSequence.PanelBSequence()); // panelB 천천히 직진
                thirdSeq.Play();
            });

            secondSeq.Play();
        });

        firstSeq.Play();
    }
}
