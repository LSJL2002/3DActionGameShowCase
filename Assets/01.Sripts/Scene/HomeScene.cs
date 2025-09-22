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
        AudioManager.Instance.PlayBGM("2");
    }

    private async void ShowUI()
    {
        await UIManager.Instance.Show<TitleUI>();
    }
}
