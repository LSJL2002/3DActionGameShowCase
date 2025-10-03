using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class GameScene : SceneBase
{
    private AsyncOperationHandle<GameObject> minimapCameraHandle; // 미니맵 카메라 오브젝트 핸들
    private AsyncOperationHandle<GameObject> minimapPlayerIconHandle; // 미니맵 플레이어 아이콘 오브젝트 핸들

    protected override void Start()
    {
        base.Start();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { DelayMethod(); });

        AudioManager.Instance.PlayBGM("1");

        // 타임라인매니저 최초 인스턴스용 호출
        TimeLineManager timeLineManager = TimeLineManager.Instance;
    }

    public async void DelayMethod()
    {
        // 게임UI, 미니맵UI 호출
        await UIManager.Instance.Show<GameUI>();
        await UIManager.Instance.Show<MiniMapUI>();

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
