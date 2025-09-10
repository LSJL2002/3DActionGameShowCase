using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoUI : UIBase
{
    public void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
