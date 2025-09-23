using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ShowUI();
        AudioManager.Instance.PlayBGM("1");
    }

    private async void ShowUI()
    {
        await UIManager.Instance.Show<GameUI>();
    }
}
