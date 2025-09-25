using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KeySettingUI : UIBase
{
    [SerializeField] private GameObject quitButton;

    protected override void OnEnable()
    {
        if (SceneLoadManager.Instance.nowSceneIndex == 2)
        {
            quitButton.SetActive(false);
        }
        else
        {
            quitButton.SetActive(true);
        }
    }

    public async void OnClickButton(string str)
    {
        AudioManager.Instance.PlaySFX("ButtonSoundEffect");

        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":

                // Home씬이라면, HomeUI 팝업 / 그 외 씬이라면, I 팝업
                if (SceneLoadManager.Instance.nowSceneIndex == 2)
                {
                    await UIManager.Instance.Show<HomeUI>();
                }
                else
                {
                    await UIManager.Instance.Show<PauseUI>();
                }
                Hide();
                break;

            case "Quit":
                // Home씬으로 돌아가기 (거기서 종료가능)
                SceneLoadManager.Instance.LoadScene(2);
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
