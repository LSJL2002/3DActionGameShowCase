public class TimeLine_BattleStart : TimeLineBase
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
