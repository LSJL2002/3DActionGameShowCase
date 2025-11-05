using UnityEngine;

public class InterfaceUI : UIBase
{
    protected override void Awake()
    {
        base.Awake();

        PlayerManager.Instance.EnableInput(false);
        EventsManager.Instance.TriggerEvent(GameEvent.OnMenu);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickButton("Quit");
        }
    }

    public void OnClickButton(string str)
    {
        switch (str)
        {
            case "Quit":
                PlayerManager.Instance.EnableInput(true);
                EventsManager.Instance.TriggerEvent(GameEvent.OnMenu);
                break;
        }
        // 현재 팝업창 닫기
        Hide();
    }
}
