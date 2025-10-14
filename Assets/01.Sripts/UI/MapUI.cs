using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : UIBase
{
    public async void OnClickButton(string str)
    {
        AudioManager.Instance.PlaySFX("ButtonSoundEffect");

        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                // UI매니저의 '이전UI' 변수를 찾아 활성화
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                // UI매니저의 '현재UI' 변수에 이전 변수를 저장
                UIManager.Instance.currentUI = UIManager.Instance.previousUI;
                break;

            // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
            case "Option":
                await UIManager.Instance.Show<SoundSettingUI>();
                break;

            // Home씬으로 돌아가기 (거기서 종료가능)
            case "Quit":
                SceneLoadManager.Instance.LoadScene(1);
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
