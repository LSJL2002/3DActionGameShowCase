using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ShowHomeUI();
    }

    private async void ShowHomeUI()
    {
        await UIManager.Instance.Show<HomeUI>();
    }
}
