using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestScene : SceneBase
{
    protected override void Start()
    {
        base.Start();
        ShowUI();
    }

    private async void ShowUI()
    {
        //await UIManager.Instance.Show<InventoryUI>();
        await UIManager.Instance.Show<TownUI>();
    }
}
