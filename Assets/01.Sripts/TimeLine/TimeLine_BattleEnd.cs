public class TimeLine_BattleEnd : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        transform.position = BattleManager.Instance.currentZone.transform.position;

        playableDirector.Play();

        playableDirector.stopped += OnTimeLineStop;
    }

    protected async override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;
        await UIManager.Instance.Show<TutorialUI>();
        UIManager.Instance.Get<TutorialUI>().PlayBossBeforeSelection(SceneType.Boss_1_Death);
    }
}
