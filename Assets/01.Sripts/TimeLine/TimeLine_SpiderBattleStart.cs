using UnityEngine;
using UnityEngine.Playables;

public class TimeLine_SpiderBattleStart : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.Play();

        playableDirector.stopped += OnTimeLineStop;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;
    }
}
