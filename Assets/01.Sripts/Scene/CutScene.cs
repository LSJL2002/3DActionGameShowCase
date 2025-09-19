using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScene : SceneBase
{
    public PlayableDirector _director;

    protected override void OnEnable()
    {
        // 스크립트가 활성화될 때 이벤트 등록
        _director.stopped += OnTimelineFinished;
    }

    protected override void OnDisable()
    {
        // 스크립트가 비활성화될 때 이벤트 해제
        _director.stopped -= OnTimelineFinished;
    }

    protected override void Start()
    {
        base.Start();
        ShowUI();
        try { AudioManager.Instance.PlayBGM("2"); }
        catch { Debug.LogError("BGM 없음"); }
    }

    // 타임라인이 끝났을 때 호출되는 메서드
    private void OnTimelineFinished(PlayableDirector director)
    {
        //SceneLoadManager.Instance.LoadScene(3); // 인트로씬1로 전환
    }

    private async void ShowUI()
    {
        await UIManager.Instance.Show<HomeUI>();
    }
}
