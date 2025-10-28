using UnityEngine;

public class TimeLine_DrainAbility : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.stopped += OnTimeLineStop;

        // 타임라인의 위치를 현재 플레이어 캐릭터 위치로 이동
        Vector3 playerPosition = PlayerManager.Instance.ActiveCharacter.transform.position;
        transform.position = playerPosition;

        playableDirector.Play();
    }

    protected async override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;

        await UIManager.Instance.Show<TutorialUI>();
        UIManager.Instance.Get<TutorialUI>().PlayBossAfterSelection(SceneType.Boss_1);
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("InGameBGM");
    }
}
