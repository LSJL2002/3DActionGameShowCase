using DG.Tweening;
using UnityEngine;

// GameUI의 Base
public partial class GameUI : UIBase, IInterfaceOpen
{
    [SerializeField] CanvasGroup gameUICanvasGroup;

    protected override void OnEnable()
    {
        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { gameUICanvasGroup.DOFade(1f, 1f); });

        OnEnablePlayer();
        OnEnableEnemy();
        OnEnableIcon();
    }

    protected override void Start()
    {
        base.Start();

        OnStartPlayer();
        EventListen();
    }

    protected override void Update()
    {
        base.Update();

        UpdateIcon();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnDisablePlayer();
        OnDisableEnemy();
        OnDisableIcon();
    }

    public void EventListen()
    {
        EventsManager.Instance.StopListening(GameEvent.OnMenu, Interact);
        EventsManager.Instance.StopListening(GameEvent.OnESCButton, Interact);
        EventsManager.Instance.StartListening(GameEvent.OnMenu, Interact);
        EventsManager.Instance.StartListening(GameEvent.OnESCButton, Interact);
    }

    public void Interact()
    {
        gameUICanvasGroup.alpha = (gameUICanvasGroup.alpha == 0f) ? 1f : 0f;
    }
}
