using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MiniMapUI : UIBase
{
    public enum MapMode
    {
        MiniMap,
        FullMap,
    }

    private MapMode previousMapMode = MapMode.MiniMap;
    private AsyncOperationHandle<GameObject> minimapCameraHandle; // 미니맵 카메라 오브젝트 핸들
    private AsyncOperationHandle<GameObject> minimapPlayerIconHandle; // 미니맵 플레이어 아이콘 오브젝트 핸들

    [SerializeField] CanvasGroup miniMapCanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { DelayMethod(); });
    }

    // 테스트용! -------------------------------------------------------------------------------------
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ChangeMapMode();
        }
    }
    // 테스트용! -------------------------------------------------------------------------------------

    public async void DelayMethod()
    {
        // 미니맵 카메라, 플레이어 아이콘 어드레서블로 생성
        minimapPlayerIconHandle = Addressables.InstantiateAsync("Minimap_PlayerIcon", PlayerManager.Instance.ActiveCharacter.transform);
        minimapCameraHandle = Addressables.InstantiateAsync("MinimapCamera");

        // 두 작업이 모두 완료될 때까지 기다림
        await UniTask.WhenAll
        (
            minimapPlayerIconHandle.ToUniTask(),
            minimapCameraHandle.ToUniTask()
        );

        // 각 UI 알파값 1로 변경(페이드인 효과)
        miniMapCanvasGroup.DOFade(1f, 1f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // 미니맵 카메라, 플레이어 아이콘 오브젝트 언로드
        if (minimapPlayerIconHandle.IsValid())
            Addressables.ReleaseInstance(minimapPlayerIconHandle);
        if (minimapCameraHandle.IsValid())
            Addressables.ReleaseInstance(minimapCameraHandle);
    }

    // 맵모드 전환
    public void ChangeMapMode()
    {
        switch(previousMapMode)
        {
            case MapMode.MiniMap:
                previousMapMode = MapMode.FullMap;
                break;

            case MapMode.FullMap:
                previousMapMode = MapMode.MiniMap;
                break;
        }
    }
}
