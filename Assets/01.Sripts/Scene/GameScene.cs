using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameScene : SceneBase
{
    private AsyncOperationHandle<GameObject> minimapCameraHandle; // 미니맵 카메라 오브젝트 핸들
    private AsyncOperationHandle<GameObject> minimapPlayerIconHandle; // 미니맵 플레이어 아이콘 오브젝트 핸들

    CanvasGroup gameUICanvasGroup;
    CanvasGroup miniMapCanvasGroup;
    CanvasGroup attackGaugeCanvasGroup;

    protected async override void Awake()
    {
        base.Awake();

        // 각 UI 로드
        await UIManager.Instance.Show<GameUI>();
        await UIManager.Instance.Show<AttackGaugeUI>();
        await UIManager.Instance.Show<MiniMapUI>();

        // 각 UI의 CanvasGroup 컴포넌트 가져오기
        gameUICanvasGroup = UIManager.Instance.Get<GameUI>().GetComponent<CanvasGroup>();
        miniMapCanvasGroup = UIManager.Instance.Get<MiniMapUI>().GetComponent<CanvasGroup>();
        attackGaugeCanvasGroup = UIManager.Instance.Get<AttackGaugeUI>().GetComponent<CanvasGroup>();
    }

    protected override void Start()
    {
        base.Start();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { DelayMethod(); });

        AudioManager.Instance.PlayBGM("1");

        // 타임라인매니저 최초 인스턴스용 호출
        TimeLineManager timeLineManager = TimeLineManager.Instance;
        MapManager.Instance.ResetZones();
        BattleManager.Instance.ResetBattleState();
    }

    public async void DelayMethod()
    {
        // 각 UI 알파값 1로 변경(페이드인 효과)
        gameUICanvasGroup.DOFade(1f, 1f);
        miniMapCanvasGroup.DOFade(1f, 1f);
        attackGaugeCanvasGroup.DOFade(1f, 1f);

        // 미니맵 카메라, 플레이어 아이콘 어드레서블로 생성
        minimapPlayerIconHandle = Addressables.InstantiateAsync("Minimap_PlayerIcon", PlayerManager.Instance.transform);
        minimapCameraHandle = Addressables.InstantiateAsync("MinimapCamera");

        // UI매니저의 튜토리얼 재생 여부 확인 후 재생
        if (UIManager.Instance.tutorialEnabled)
        {
            await UIManager.Instance.Show<TutorialUI>();
            UIManager.Instance.Get<TutorialUI>().PlayDialogue(SceneType.Tutorial);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // 미니맵 카메라, 플레이어 아이콘 오브젝트 언로드
        Addressables.ReleaseInstance(minimapPlayerIconHandle);
        Addressables.ReleaseInstance(minimapCameraHandle);
    }
}