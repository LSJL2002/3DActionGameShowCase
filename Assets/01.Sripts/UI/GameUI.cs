using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "Pause":
                // 게임매니저의 게임 일시정지 메서드를 호출
                GameManager.Instance.PauseGame(true);

                // 일시정지 UI 팝업
                // await UIManager.Instance.Show<PauseUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
