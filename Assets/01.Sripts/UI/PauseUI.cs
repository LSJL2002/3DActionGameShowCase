using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                break;

            // 현재 게임씬 다시 시작
            case "ReStart":
                SceneLoadManager.Instance.ChangeScene(SceneLoadManager.Instance.NowSceneIndex);
                break;

            // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
            case "Option":
                await UIManager.Instance.Show<OptionUI>();
                break;

            // 홈씬으로 돌아가기
            case "BackHome":
                SceneLoadManager.Instance.ChangeScene(1);
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
