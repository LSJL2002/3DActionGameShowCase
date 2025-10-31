public class InterfaceUI : UIBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        EventsManager.Instance.StartListening(GameEvent.OnESCButton, OnInterfaceUI);
    }

    private void OnInterfaceUI()
    {
        EventsManager.Instance.StopListening(GameEvent.OnESCButton, OnInterfaceUI);
        Hide();
    }
}
