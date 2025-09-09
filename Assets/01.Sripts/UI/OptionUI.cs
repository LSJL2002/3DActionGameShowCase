using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : UIBase
{
    public async void OnClickExit()
    {
        // ÆË¾÷¸ÞÀÎ ¿­±â
        await UIManager.Instance.Show<HomeUI>();

        // ÇöÀç ÆË¾÷Ã¢ ´Ý±â
        Hide();
    }
}
