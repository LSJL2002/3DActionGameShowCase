using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLine_DrainAbility : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.stopped += OnTimeLineStop;
        playableDirector.stopped += SetToDay => MapManager.Instance.GetComponent<SkyboxBlendController>().SetToDay();

        // 타임라인의 위치를 현재 플레이어 캐릭터 위치로 이동
        float height = PlayerManager.Instance.GetComponent<CharacterController>().height;
        Vector3 playerPosition = PlayerManager.Instance.transform.position;
        Vector3 newposition = new Vector3(playerPosition.x, playerPosition.y + height, playerPosition.z);
        this.transform.position = newposition;
        
        playableDirector.Play();
    }

    // 타임라인 정지시 호출 될 함수 (타임라인 정지 이벤트에 구독)
    // 이 타임라인은 재사용되기 때문에 릴리즈하지 않고 비활성화 하도록 함수를 override 함.
    protected override void OnTimeLineStop(PlayableDirector director)
    {
        playableDirector.stopped -= OnTimeLineStop;
        playableDirector.stopped -= SetToDay => MapManager.Instance.GetComponent<SkyboxBlendController>().SetToDay();
        TimeLineManager.Instance.Hide(gameObject.name);

        UIManager.Instance.Get<TutorialUI>().PlayBossAfterSelection(SceneType.Boss_1);
    }
}
