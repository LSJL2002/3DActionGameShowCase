using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DamageEffectUI : UIBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        uiType = UIType.Popup;

        // n초 대기 후 실행
        DOVirtual.DelayedCall(0.3f, () => { Hide(); });
    }
}