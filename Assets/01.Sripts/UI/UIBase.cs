using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;

    public virtual void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }

    protected virtual void Awake() { }

    protected virtual void OnEnable() { }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDestroy() { }
}
