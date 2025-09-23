using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class LetterBox : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    [Header("Animation Settings")]
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private Ease slideEase = Ease.InOutSine;

    private Vector2 topInitialPos;
    private Vector2 bottomInitialPos;
    private int preparedCount = 0; // 모든 영상 준비 완료 체크

    private void Awake()
    {
        // 초기 위치 기록
        topInitialPos = topBar.anchoredPosition;
        bottomInitialPos = bottomBar.anchoredPosition;

    }

    private void OnEnable()
    {
        preparedCount = 0;
    }

    private void LetterBoxIn()
    {
        float topHeight = topBar.sizeDelta.y;
        float bottomHeight = bottomBar.sizeDelta.y;

        // 처음 화면 밖 위치
        topBar.anchoredPosition = topInitialPos + new Vector2(0, topHeight);
        bottomBar.anchoredPosition = bottomInitialPos - new Vector2(0, bottomHeight);

        // 부드럽게 들어오기
        topBar.DOAnchorPos(topInitialPos, slideDuration).SetEase(slideEase);
        bottomBar.DOAnchorPos(bottomInitialPos, slideDuration).SetEase(slideEase);
    }

    public void LetterBoxOut()
    {
        float topHeight = topBar.sizeDelta.y;
        float bottomHeight = bottomBar.sizeDelta.y;

        // 부드럽게 밖으로 슬라이드
        topBar.DOAnchorPos(topInitialPos + new Vector2(0, topHeight), slideDuration).SetEase(slideEase);
        bottomBar.DOAnchorPos(bottomInitialPos - new Vector2(0, bottomHeight), slideDuration).SetEase(slideEase)
            .OnComplete(() =>
            {

                // GameObject 비활성화
                gameObject.SetActive(false);
            });
    }

    private void OnDisable()
    {
    }
}