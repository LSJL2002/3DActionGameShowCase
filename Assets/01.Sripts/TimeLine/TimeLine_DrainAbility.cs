using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLine_DrainAbility : TimeLineBase
{
    [SerializeField] private PlayableDirector playableDirector;

    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.stopped += OnTimeLineStop;

        // 타임라인의 위치를 현재 플레이어 캐릭터 위치로 이동
        float height = PlayerManager.Instance.GetComponent<CharacterController>().height;
        Vector3 playerPosition = PlayerManager.Instance.transform.position;
        Vector3 newposition = new Vector3(playerPosition.x, playerPosition.y + height, playerPosition.z);
        this.transform.position = newposition;

        playableDirector.Play();
    }

    protected override void OnTimeLineStop(PlayableDirector director)
    {
        TimeLineManager.Instance.Hide(gameObject.name);
        playableDirector.stopped -= OnTimeLineStop;
    }
}
