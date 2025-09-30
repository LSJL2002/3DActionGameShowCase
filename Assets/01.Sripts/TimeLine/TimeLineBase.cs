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

    public void OnSkipButton()
    {
        // 타임라인의 재생 시간을 '총 길이 - 아주 작은 값'으로 설정
        // playableDirector.duration : 타임라인의 총 재생 시간(초)
        // 1 / GetSpeed() : 1 프레임을 재생하는 데 걸리는 시간(프레임 간격)을 의미
        double endTime = playableDirector.duration - (1 / playableDirector.playableGraph.GetRootPlayable(0).GetSpeed());

        // 설정된 시간(endTime)으로 타임라인을 이동
        playableDirector.time = endTime;
        
        playableDirector.Play();
    }
}

