using System;
using UnityEngine;

public class GameClearDoor : MonoBehaviour, IInteractable
{
    private string clearDoorText;


    private void OnEnable()
    {
        clearDoorText = $"Press Key.F";
    }
    public string GetInteractPrompt()
    {
        return clearDoorText;
    }

    public void OnInteract()
    {
        clearDoorText = string.Empty;
        PlayerManager.Instance.EnableInput(true); // 플레이어 입력 잠금
        Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
        Cursor.visible = true; // 커서 보이게

        UIManager.Instance.currentDecisionState = DecisionState.GameClear;

        SelectItem();
        AnalyticsManager.SendFunnelStep("60");
    }

    public async void SelectItem()
    {
        DecisionButtonUI decisionUI = await UIManager.Instance.Show<DecisionButtonUI>();

        DecisionButtonUI decisionButtonUI = UIManager.Instance.Get<DecisionButtonUI>();

        // 이벤트 구독을 위한 델리게이트 변수 생성
        Action<bool> onDecisionMadeCallback = null;
        onDecisionMadeCallback = async (isConfirmed) =>
        {
            // 확인UI에서 허가를 받았다면,
            if (isConfirmed)
            {
                SceneLoadManager.Instance.ChangeScene(1);
            }
            else
            {
                clearDoorText = $"Press Key.F";
            }

                // 이벤트 구독 해제
                decisionUI.OnDecisionMade -= onDecisionMadeCallback;
        };

        // 이벤트 구독
        decisionUI.OnDecisionMade += onDecisionMadeCallback;
    }
}
