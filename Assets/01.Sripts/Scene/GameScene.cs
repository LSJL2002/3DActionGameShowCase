using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameScene : SceneBase
{
    bool hasData;

    protected override void Awake()
    {
        base.Awake();
        // 타임라인매니저 최초 인스턴스용 호출
        TimeLineManager timeLineManager = TimeLineManager.Instance;
    }

    protected async override void Start()
    {
        base.Start();
        LoadingManager.Instance.SetLoadingPanel(true); // 로딩 UI 켜기
        await MapManager.Instance.LoadMap();
        await Task.Yield();
        //await MapManager.Instance.LoadAscync("Character");
        //await Task.Yield();
        //await MapManager.Instance.LoadAscync("PlayerCamera");
        LoadingManager.Instance.SetLoadingPanel(false); // 로딩 UI 끄기
        OnGameSceneStart(); // 게임 씬 시작
    }

    public async void OnGameSceneStart()
    {
        hasData = SaveManager.Instance.LoadData();

        AudioManager.Instance.PlayBGM("InGameBGM");

        if (!hasData || SaveManager.Instance.playerData.LastClearStage == 0)
        {
            GameManager.Instance.gameMode = eGameMode.NewGame;
        }
        MapManager.Instance.ResetZones();
        BattleManager.Instance.ResetBattleState();

        // 각 UI 로드
        await UIManager.Instance.Show<GameUI>();
        await UIManager.Instance.Show<AttackGaugeUI>();
        await UIManager.Instance.Show<MiniMapUI>();
        await UIManager.Instance.Show<TutorialUI>();
    }
}