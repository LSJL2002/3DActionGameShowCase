using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UIType
{
    GameUI,
    Screen,
    Popup,
    Dialog,
}

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;
    public UIType uiType = UIType.GameUI;

    public virtual void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }

    protected virtual void Awake() { }

    protected virtual void OnEnable() 
    {
        Cursor.lockState = CursorLockMode.None;
    }

    protected virtual void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
    }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDestroy() { }
}
