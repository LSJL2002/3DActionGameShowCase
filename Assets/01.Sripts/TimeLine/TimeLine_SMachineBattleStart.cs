using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLine_SMachineBattleStart : TimeLineBase
{
    private System.Action<PlayableDirector> setToNightHandler;
    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.Play();

        playableDirector.stopped += OnTimeLineStop;

        setToNightHandler = _ => MapManager.Instance.GetComponent<SkyboxBlendController>().SetToNight();
        playableDirector.stopped += setToNightHandler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;
        playableDirector.stopped -= setToNightHandler;
    }
}
