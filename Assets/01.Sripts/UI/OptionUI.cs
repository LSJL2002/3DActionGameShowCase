using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : UIBase
{
    public void OnClickExit(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                break;

            case "Quit":
                // Home씬으로 돌아가기 (거기서 종료가능)
                SceneLoadManager.Instance.ChangeScene(1);
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
