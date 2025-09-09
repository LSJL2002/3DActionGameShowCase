using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupA : UIBase
{
    public async void OnClickExit()
    {
        // ÆË¾÷¸ÞÀÎ ¿­±â
        await UIManager.Instance.Show<PopupMain>();

        // ÇöÀç ÆË¾÷Ã¢ ´Ý±â
        Hide();
    }
}
