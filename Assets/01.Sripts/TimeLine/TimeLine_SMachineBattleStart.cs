using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLine_SMachineBattleStart : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.Play();

        playableDirector.stopped += OnTimeLineStop;
        playableDirector.stopped += SetToNight => MapManager.Instance.GetComponent<SkyboxBlendController>().SetToNight();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;
        playableDirector.stopped -= SetToNight => MapManager.Instance.GetComponent<SkyboxBlendController>().SetToNight();
    }
}
