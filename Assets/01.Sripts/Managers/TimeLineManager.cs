using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// 해당 씬에만 존재할 타임라인 매니저
public class TimeLineManager : Singleton<TimeLineManager>
{
    private static TimeLineManager _instance;

    public PlayableDirector timelineDirector;

    // 인스턴스를 가져오는 프로퍼티
    public static new TimeLineManager Instance
    {
        get
        {
            // 인스턴스가 없으면 씬에서 찾기
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeLineManager>();
            }
            return _instance;
        }
    }

    protected override void Awake()
    {
        // 씬 내에 이미 인스턴스가 존재하면 현재 오브젝트 파괴
        if (_instance != null)
        {
            // 현재 인스턴스와 기존 인스턴스가 다르면 경고 로그와 함께 파괴
            if (_instance != this)
            {
                Debug.LogWarning("씬에 이미 다른 TimeLineManager 인스턴스가 존재. 중복 인스턴스를 파괴.");
                Destroy(gameObject);
                return;
            }
        }

        // 싱글톤 인스턴스 설정
        _instance = this;

        try
        {
            timelineDirector = FindObjectOfType<PlayableDirector>();
        }
        catch { Debug.Log("실패"); };
    }

    public void PlayTimeLine()
    {
        timelineDirector.Play();
    }
}
