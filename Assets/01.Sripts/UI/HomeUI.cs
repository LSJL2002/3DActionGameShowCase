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
        switch (str)
        {
            case "NewGame":
                // 새 게임 시작
                GameManager.Instance.gameMode = eGameMode.NewGame;
                SceneLoadManager.Instance.LoadScene(2);
                break;

            case "LoadStart":
                // 기존 게임을 로드
                GameManager.Instance.gameMode = eGameMode.LoadGame;
                SceneLoadManager.Instance.LoadScene(2);
                break;

            case "OptionUI":
                // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
                await UIManager.Instance.Show<SoundSettingUI>();
                break;

            case "Quit":
                // 어플리케이션 종료
                Application.Quit();
                break;
        }
        // 현재 팝업창 닫기
        Hide();
    }
}
