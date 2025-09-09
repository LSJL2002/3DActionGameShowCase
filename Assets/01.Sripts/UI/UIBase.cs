using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;

    public virtual void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }
}
