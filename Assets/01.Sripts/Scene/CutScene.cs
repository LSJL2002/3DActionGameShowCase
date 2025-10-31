public class CutScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();

        AudioManager.Instance.StopBGM();
    }
}
