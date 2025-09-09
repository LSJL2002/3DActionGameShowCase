using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMain : UIBase
{
    public async void OnClickButton(string str)
    {
        switch(str)
        {
            case "A":
                // IU매니저의 Show 메서드를 호출하여 PopupA를 화면에 표시
                await UIManager.Instance.Show<PopupA>();
                break;

            case "B":
                // IU매니저의 Show 메서드를 호출하여 PopupB를 화면에 표시
                await UIManager.Instance.Show<PopupB>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
