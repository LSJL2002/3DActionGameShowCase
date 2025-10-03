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

        // -------------------------
        // 1차 시퀀스 실행
        Sequence mainSeq = DOTween.Sequence();

        // 1차 시퀀스
        mainSeq.Append(firstSequence.PlaySequence());

        // 2차 시퀀스 + PanelB 동시에 시작
        var secondSeq = secondSequence.PlaySequence();
        mainSeq.Append(secondSeq);

        // PanelBSequence를 동시에 실행
        var panelBSeq = secondSequence.PanelBSequence();
        if (panelBSeq != null)
        {
            // Append 후 Join하면 secondSeq와 동시에 시작
            mainSeq.Join(panelBSeq);
        }

        // PanelB 위치 전달 후 3차 시퀀스
        mainSeq.AppendCallback(() =>
        {
            if (thirdSequence.panelB != null && secondSequence != null)
            {
                thirdSequence.panelB.anchoredPosition = secondSequence.PanelBOriginalPos;
                thirdSequence.panelB.gameObject.SetActive(true);
            }
        });

        mainSeq.Append(thirdSequence.PlaySequence());
        mainSeq.Append(thirdSequence.PanelBSequence());

        mainSeq.SetUpdate(true);
        mainSeq.Play();
    }
}