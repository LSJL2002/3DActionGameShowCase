using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : UIBase
{
    public void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                // UI매니저의 '이전UI' 변수를 찾아 활성화
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                // UI매니저의 '현재UI' 변수에 이전 변수를 저장
                UIManager.Instance.currentUI = UIManager.Instance.previousUI;
                break;

            case "Quit":
                // Home씬으로 돌아가기 (거기서 종료가능)
                SceneLoadManager.Instance.LoadScene(2);
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
