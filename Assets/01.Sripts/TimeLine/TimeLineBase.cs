using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineBase : MonoBehaviour
{
    Camera mainCamera;
    Canvas gameUICanvas;

    protected virtual void OnTimeLineStop(PlayableDirector director)
    {
        TimeLineManager.Instance.Release(gameObject.name);
    }

    protected virtual void Awake() 
    {
        mainCamera = PlayerManager.Instance.GetComponentInChildren<Camera>();
        gameUICanvas = UIManager.Instance.Get<GameUI>().canvas;
    }

    protected virtual void OnEnable() 
    {
        mainCamera.gameObject.SetActive(false);
        gameUICanvas.gameObject.SetActive(false);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() 
    {
        mainCamera.gameObject.SetActive(true);
        gameUICanvas.gameObject.SetActive(true);
    }

    protected virtual void OnDestroy() { }
}

