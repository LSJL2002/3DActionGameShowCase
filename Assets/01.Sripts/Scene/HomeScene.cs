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
        ShowPopupMain();
    }

    private async void ShowPopupMain()
    {
        await UIManager.Instance.Show<PopupMain>();
    }
}
