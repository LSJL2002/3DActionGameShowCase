using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InvenEventFirst : MonoBehaviour
{
    public RectTransform[] panels;   // [0]=왼1, [1]=오1, [2]=왼2, [3]=오2
    public float offsetX = 2.5f;     // UI 단위(pixels)로 설정하세요
    public float duration = 1f;
    public float stayTime = 0.5f;    // 화면에 머무는 시간
    public float exitMultiplier = 1.5f;   // 사라질 때 얼마나 멀리 보낼지

    private Vector2[] targetPos;
    private Vector2[] startPos;
    private Vector2[] endPos;  // 나갈 때 위치 (반대방향)
    private int[] dir; // -1 = 왼쪽에서 들어옴, +1 = 오른쪽에서 들어옴


    public Sequence PlaySequence(Action onComplete = null)
    {
        // 현재 위치 저장
        int n = panels.Length;
        targetPos = new Vector2[n];
        startPos = new Vector2[n];
        endPos = new Vector2[n];
        dir = new int[n];

        // 시작 위치 밖으로 이동
        for (int i = 0; i < n; i++)
        {
            targetPos[i] = panels[i].anchoredPosition;

            // 인덱스로 방향 결정 (짝수 = 왼쪽, 홀수 = 오른쪽)
            dir[i] = (i % 2 == 0) ? -1 : 1;
            // 시작 위치 = 도착 위치 + offset(왼/오)
            startPos[i] = targetPos[i] + new Vector2(offsetX * dir[i], 0f);
            // 사라질 위치 (멀리 보내기)
            endPos[i] = targetPos[i] + new Vector2(offsetX * -dir[i] * exitMultiplier, 0f);
            // 패널을 시작 위치로 옮겨놓음
            panels[i].anchoredPosition = startPos[i];
        }

        // 시퀀스 구성
        Sequence seq = DOTween.Sequence();

        // -------------------------
        // 모든 패널 동시에 들어오기
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == 0)
                seq.Append(
                    panels[i].DOAnchorPosX(targetPos[i].x, duration)
                             .SetEase(Ease.OutQuad)
                             .SetUpdate(true) // <--- 여기가 핵심
                );
            else
                seq.Join(
                    panels[i].DOAnchorPosX(targetPos[i].x, duration)
                             .SetEase(Ease.OutQuad)
                             .SetUpdate(true)
                );
        }

        // 화면에 잠시 머무르기
        seq.AppendInterval(stayTime).SetUpdate(true); // Interval도 unscaled 필요

        // -------------------------
        // 모든 패널 동시에 나가기
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == 0)
                seq.Append(
                    panels[i].DOAnchorPosX(endPos[i].x, duration)
                             .SetEase(Ease.InQuad)
                             .SetUpdate(true)
                );
            else
                seq.Join(
                    panels[i].DOAnchorPosX(endPos[i].x, duration)
                             .SetEase(Ease.InQuad)
                             .SetUpdate(true)
                );
        }

        seq.OnComplete(() => onComplete?.Invoke());
        return seq;
    }
}
