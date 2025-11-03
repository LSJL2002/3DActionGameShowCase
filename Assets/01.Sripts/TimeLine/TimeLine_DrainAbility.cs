using UnityEngine;

public class TimeLine_DrainAbility : TimeLineBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        playableDirector.stopped += OnTimeLineStop;

        // 타임라인의 위치를 현재 플레이어 캐릭터 위치로 이동
        Vector3 playerPosition = PlayerManager.Instance.ActiveCharacter.transform.position;
        transform.position = playerPosition;

        playableDirector.Play();
    }

    protected async override void OnDisable()
    {
        base.OnDisable();

        playableDirector.stopped -= OnTimeLineStop;

        if (UIManager.Instance == null) return;
        await UIManager.Instance.Show<TutorialUI>();
        UIManager.Instance.Get<TutorialUI>().PlayBossAfterSelection(SceneType.Boss_1);
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("InGameBGM");
        CheckPlayDrainAbility();
        BattleManager.Instance.ClearBattle();
    }
    private void CheckPlayDrainAbility()
    {
        switch (BattleManager.Instance.currentZone.id)
        {
            case 60000000:
                AnalyticsManager.SendFunnelStep("19");
                break;
            case 60000001:
                AnalyticsManager.SendFunnelStep("29");
                break;
            case 60000002:
                AnalyticsManager.SendFunnelStep("29");
                break;
            case 60000003:
                AnalyticsManager.SendFunnelStep("29");
                break;
            case 60000004:
                AnalyticsManager.SendFunnelStep("39");
                break;
            case 60000005:
                AnalyticsManager.SendFunnelStep("39");
                break;
            case 60000006:
                AnalyticsManager.SendFunnelStep("39");
                break;
            case 60000007:
                AnalyticsManager.SendFunnelStep("49");
                break;
            case 60000008:
                AnalyticsManager.SendFunnelStep("49");
                break;
            case 60000009:
                AnalyticsManager.SendFunnelStep("49");
                break;
            case 60000010:
                AnalyticsManager.SendFunnelStep("59");
                break;
        }
    }
}
