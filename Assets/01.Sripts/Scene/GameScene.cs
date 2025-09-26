using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        DOVirtual.DelayedCall(6f, async () => { await UIManager.Instance.Show<GameUI>(); });
        DOVirtual.DelayedCall(7f, async () => 
        {
            await UIManager.Instance.Show<TutorialUI>();
        }).OnComplete(() =>
            {
                UIManager.Instance.Get<TutorialUI>().PlayDialogue(SceneType.Tutorial);}
            );
        
        AudioManager.Instance.PlayBGM("1");
    }
}
