using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ShowTownUI();
    }

    private async void ShowTownUI()
    {
        await UIManager.Instance.Show<TownUI>();
    }
}
