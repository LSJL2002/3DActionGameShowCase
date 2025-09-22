using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "GameStart":
                await UIManager.Instance.Show<HomeUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
