using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MiniMapUI : UIBase
{
    public enum MapMode
    {
        MiniMap,
        FullMap,
    }

    [SerializeField] CanvasGroup miniMapCanvasGroup;
    [SerializeField] Image miniMap;

    private MapMode previousMapMode = MapMode.MiniMap;
    private AsyncOperationHandle<GameObject> minimapCameraHandle; // 미니맵 카메라 오브젝트 핸들
    private AsyncOperationHandle<GameObject> minimapPlayerIconHandle; // 미니맵 플레이어 아이콘 오브젝트 핸들
    private Camera minimapCamera;
    private Canvas minimapCanvas;
    private int firstSortingOrder;
    private Sequence fullMapsequence;

    // FullMap 위치
    private Vector2 anchorMinFullMap = new Vector2(0.5f,0.5f);
    private Vector2 anchorMaxFullMap = new Vector2(0.5f, 0.5f);
    private Vector3 positionFullMap = new Vector3(0, 0, 0);
    private Vector3 fullMapScale = new Vector3(4, 4, 4);

    // MiniMap 위치
    private Vector2 anchorMinMiniMap = new Vector2(1f, 0f);
    private Vector2 anchorMaxMiniMap = new Vector2(1f, 0f);
    private Vector3 positionMiniMap;

    protected override void Awake()
    {
        base.Awake();

        minimapCanvas = GetComponentInParent<Canvas>();
        firstSortingOrder = minimapCanvas.sortingOrder; // 첫 SortingOrder 값을 저장
        positionMiniMap = miniMap.GetComponent<RectTransform>().anchoredPosition; // 첫 포지션 위치를 저장
        SetSequence();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { DelayMethod(); });
    }

    // 확대 시퀀스를 처음 생성하는 함수
    public void SetSequence()
    {
        fullMapsequence = DOTween.Sequence();
        fullMapsequence.Append(miniMap.rectTransform.DOScale(fullMapScale, 0.5f))
            .SetAutoKill(false); // 재사용 가능하도록

        fullMapsequence.Pause(); // 생성시에는 실행하지 않음
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

        if (minimapCameraHandle.IsValid() && minimapCameraHandle.Result != null)
        {
            minimapCamera = minimapCameraHandle.Result.GetComponent<Camera>();
        }

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

        if (fullMapsequence != null)
        {
            fullMapsequence.Kill();
            fullMapsequence = null;
        }
    }

    // 맵모드 전환
    public void ChangeMapMode()
    {
        switch(previousMapMode)
        {
            case MapMode.MiniMap:
                previousMapMode = MapMode.FullMap;
                miniMap.rectTransform.anchorMin = anchorMinFullMap;
                miniMap.rectTransform.anchorMax = anchorMaxFullMap;
                miniMap.rectTransform.anchoredPosition = positionFullMap;
                minimapCamera.orthographicSize = 300;
                minimapCanvas.sortingOrder = 110; // 최대치로 변경
                fullMapsequence.Restart();
                break;

            case MapMode.FullMap:
                previousMapMode = MapMode.MiniMap;
                miniMap.rectTransform.anchorMin = anchorMinMiniMap;
                miniMap.rectTransform.anchorMax = anchorMaxMiniMap;
                miniMap.rectTransform.anchoredPosition = positionMiniMap;
                minimapCamera.orthographicSize = 40;
                minimapCanvas.sortingOrder = firstSortingOrder; // 처음 값으로 변경
                fullMapsequence.PlayBackwards();
                break;
        }
    }
}
