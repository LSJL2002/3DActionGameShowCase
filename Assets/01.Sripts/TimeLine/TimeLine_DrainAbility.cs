using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLine_DrainAbility : TimeLineBase
{
    private System.Action<PlayableDirector> setToDayHandler;

    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.stopped += OnTimeLineStop;

        setToDayHandler = _ => MapManager.Instance.GetComponent<SkyboxBlendController>().SetToDay();
        playableDirector.stopped += setToDayHandler;
        // 타임라인의 위치를 현재 플레이어 캐릭터 위치로 이동
        Vector3 playerPosition = PlayerManager.Instance.ActiveCharacter.transform.position;
        transform.position = playerPosition;

        playableDirector.Play();
    }

    protected async override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;
        playableDirector.stopped -= setToDayHandler;

        await UIManager.Instance.Show<TutorialUI>();
        UIManager.Instance.Get<TutorialUI>().PlayBossAfterSelection(SceneType.Boss_1);
    }
}
