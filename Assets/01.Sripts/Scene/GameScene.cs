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
        ShowGameUI();
    }

    private async void ShowGameUI()
    {
        await UIManager.Instance.Show<GameUI>();
    }
}
