using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch(str)
        {
            case "GameStart":
                // 게임매니저의 게임시작 메서드를 호출
                GameManager.Instance.StartGame();
                break;

            case "OptionUI":
                // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
                await UIManager.Instance.Show<OptionUI>();
                break;

            case "GameExit":
                // 어플리케이션 종료
                Application.Quit();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
