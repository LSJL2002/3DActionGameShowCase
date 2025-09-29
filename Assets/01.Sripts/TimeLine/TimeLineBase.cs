using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineBase : MonoBehaviour
{
    private Camera mainCamera;
    private Canvas gameUICanvas;
    [SerializeField] protected PlayableDirector playableDirector;

    protected virtual void OnTimeLineStop(PlayableDirector director)
    {
        TimeLineManager.Instance.Release(gameObject.name);
    }

    protected virtual void Awake() 
    {
        mainCamera = PlayerManager.Instance.camera.GetComponentInChildren<Camera>();
        gameUICanvas = UIManager.Instance.Get<GameUI>().canvas;
    }

    protected virtual void OnEnable() 
    {
        mainCamera.gameObject.SetActive(false);
        gameUICanvas.gameObject.SetActive(false);
        AudioManager.Instance.StopBGM();

        PlayerManager.Instance.EnableInput(false); // 마우스 커서 보이게
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() 
    {
        mainCamera.gameObject.SetActive(true);
        gameUICanvas.gameObject.SetActive(true);

        PlayerManager.Instance.EnableInput(true); // 마우스 락
    }

    protected virtual void OnDestroy() { }

    public void OnStopButton()
    {
        playableDirector.Stop();
    }
}

