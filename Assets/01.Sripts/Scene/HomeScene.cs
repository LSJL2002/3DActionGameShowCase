using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HomeScene : SceneBase
{
    protected override void Start()
    {
        base.Start();
        ShowUI();
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("2");

        Cursor.lockState = CursorLockMode.None;
    }

    private async void ShowUI()
    {
        await UIManager.Instance.Show<TitleUI>();
    }
}
