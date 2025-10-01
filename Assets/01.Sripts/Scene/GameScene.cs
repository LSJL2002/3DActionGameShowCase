using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { DelayMethod(); });

        AudioManager.Instance.PlayBGM("1");

        // 타임라인매니저 최초 인스턴스용 호출
        TimeLineManager timeLineManager = TimeLineManager.Instance;
    }

    public async void DelayMethod()
    {
        await UIManager.Instance.Show<GameUI>();

        // UI매니저의 튜토리얼 재생 여부 확인 후 재생
        if (UIManager.Instance.tutorialEnabled)
        {
            await UIManager.Instance.Show<TutorialUI>();
            UIManager.Instance.Get<TutorialUI>().PlayDialogue(SceneType.Tutorial);
            UIManager.Instance.tutorialEnabled = false;
        }
    }
}
