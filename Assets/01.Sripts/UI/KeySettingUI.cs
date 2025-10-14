using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KeySettingUI : UIBase
{
    [SerializeField] private GameObject returnButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (SceneLoadManager.Instance.nowSceneIndex == 0)
        {
            returnButton.SetActive(true);
        }
        else
        {
            returnButton.SetActive(false);
        }
    }

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<HomeUI>();
                Hide();
                break;

            case "Quit":
                // Home씬으로 돌아가기 (거기서 종료가능)
                SceneLoadManager.Instance.LoadScene(1);
                Hide();
                break;

            // 사운드세팅 UI
            case "SoundSetting":
                await UIManager.Instance.Show<SoundSettingUI>();
                Hide();
                break;
        }

        // 현재 팝업창 닫기
        //Hide();
    }
}
