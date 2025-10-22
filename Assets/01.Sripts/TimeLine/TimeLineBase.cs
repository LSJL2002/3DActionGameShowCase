using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineBase : MonoBehaviour
{
    private Camera mainCamera;
    private CanvasGroup gameUICanvasGroup;
    private CanvasGroup miniMapUICanvasGroup;
    private CanvasGroup attackGaugeUICanvasGroup;
    [SerializeField] protected PlayableDirector playableDirector;

    protected virtual void OnTimeLineStop(PlayableDirector director)
    {
        TimeLineManager.Instance.Release(gameObject.name);
    }

    protected virtual void Awake() 
    {
        mainCamera = PlayerManager.Instance._camera.GetComponentInChildren<Camera>();
        gameUICanvasGroup = UIManager.Instance.Get<GameUI>().GetComponent<CanvasGroup>();
        miniMapUICanvasGroup = UIManager.Instance.Get<MiniMapUI>().GetComponent<CanvasGroup>();
        attackGaugeUICanvasGroup = UIManager.Instance.Get<AttackGaugeUI>().GetComponent<CanvasGroup>();
    }

    protected virtual void OnEnable() 
    {
        if(mainCamera != null) 
        mainCamera.gameObject.SetActive(false);
        gameUICanvasGroup.alpha = 0f;
        miniMapUICanvasGroup.alpha = 0f;
        attackGaugeUICanvasGroup.alpha = 0f;
        AudioManager.Instance.StopBGM();

        PlayerManager.Instance.EnableInput(false); // 마우스 커서 보이게
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() 
    {
        if(mainCamera != null)
        mainCamera.gameObject.SetActive(true);
        gameUICanvasGroup.alpha = 1f;
        miniMapUICanvasGroup.alpha = 1f;
        attackGaugeUICanvasGroup.alpha = 1f;

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

