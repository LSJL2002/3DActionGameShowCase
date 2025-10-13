using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUI : UIBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        uiType = UIType.GameUI;
    }
}
