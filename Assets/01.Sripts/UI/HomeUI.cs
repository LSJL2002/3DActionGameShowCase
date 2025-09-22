using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : UIBase
{
    [SerializeField] private CanvasGroup newGameButton;
    [SerializeField] private CanvasGroup loadGameButton;
    [SerializeField] private CanvasGroup optionButton;
    [SerializeField] private CanvasGroup quitButton;

    public async void OnClickButton(string str)
    {
        switch(str)
        {
            case "NewGame":
                // 새 게임 시작
                SceneLoadManager.Instance.LoadScene(3);
                break;

            case "LoadStart":
                // 기존 게임을 로드
                break;

            case "OptionUI":
                // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
                await UIManager.Instance.Show<OptionUI>();
                break;

            case "Quit":
                // 어플리케이션 종료
                Application.Quit();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }

    protected override void Awake()
    {
        newGameButton.alpha = 0f;
        loadGameButton.alpha = 0f;
        optionButton.alpha = 0f;
        quitButton.alpha = 0f;
    }

    protected override void Start()
    {
        base.Start();
        newGameButton.DOFade(1f, 1.0f);
        loadGameButton.DOFade(1f, 2.0f);
        optionButton.DOFade(1f, 3.0f);
        quitButton.DOFade(1f, 4.0f);
    }
}
