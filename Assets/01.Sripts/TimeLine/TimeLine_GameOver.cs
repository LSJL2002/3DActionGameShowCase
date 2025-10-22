using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeLine_GameOver : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        AudioManager.Instance.PlayBGM("3");

        this.transform.position = BattleManager.Instance.currentZone.transform.position + Vector3.up*2;

        playableDirector.Play();
    }

    public void OnClickButton()
    {
        TimeLineManager.Instance.Release(gameObject.name);
        
        SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single); // Home씬으로 전환
    }
}
