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

    [SerializeField] CanvasGroup miniMapCanvasGroup;
    [SerializeField] RectTransform miniMapRectTransform;

    private MapMode currentMapMode = MapMode.MiniMap;
    private AsyncOperationHandle<GameObject> minimapCameraHandle; // 미니맵 카메라 오브젝트 핸들
    private AsyncOperationHandle<GameObject> minimapPlayerIconHandle; // 미니맵 플레이어 아이콘 오브젝트 핸들
    private Camera minimapCamera;
    private Canvas minimapCanvas;
    private int firstSortingOrder;
    private Sequence fullMapSequence;
    private const float animationDuration = 0.5f;

    // FullMap 관련
    private readonly Vector2 anchorMinFullMap = new Vector2(0.5f,0.5f);
    private readonly Vector2 anchorMaxFullMap = new Vector2(0.5f, 0.5f);
    private readonly Vector2 anchoredPositionFullMap = new Vector2(0, 0);
    private readonly Vector3 fullMapScale = new Vector3(4, 4, 4);
    private const int fullMapSortingOrder = 110;
    private const float fullMapCameraSize = 300f; // 카메라 줌 설정

    // MiniMap 관련
    private readonly Vector2 anchorMinMiniMap = new Vector2(1f, 0f);
    private readonly Vector2 anchorMaxMiniMap = new Vector2(1f, 0f);
    private Vector3 anchoredPositionMiniMap;
    private const float miniMapCameraSize = 40f; // 카메라 줌 설정

    protected override void Awake()
    {
        base.Awake();

        minimapCanvas = GetComponentInParent<Canvas>();
        firstSortingOrder = minimapCanvas.sortingOrder; // 첫 SortingOrder 값을 저장
        anchoredPositionMiniMap = miniMapRectTransform.anchoredPosition; // 첫 포지션 위치를 저장
        SetSequence();

        // n초 대기 후 실행
        DOVirtual.DelayedCall(6f, () => { DelayMethod(); });
    }

    // 확대 시퀀스를 처음 생성하는 함수
    public void SetSequence()
    {
        fullMapSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Pause();

        // 정방향 (MiniMap -> FullMap) 애니메이션 정의
        fullMapSequence.Append(miniMapRectTransform.DOScale(fullMapScale, animationDuration))
            .Join(miniMapRectTransform.DOAnchorPos(anchoredPositionFullMap, animationDuration));
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

        if (minimapCamera != null)
        {
            minimapCamera.orthographicSize = miniMapCameraSize;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // 미니맵 카메라, 플레이어 아이콘 오브젝트 언로드
        if (minimapPlayerIconHandle.IsValid())
            Addressables.ReleaseInstance(minimapPlayerIconHandle);
        if (minimapCameraHandle.IsValid())
            Addressables.ReleaseInstance(minimapCameraHandle);

        if (fullMapSequence != null)
        {
            fullMapSequence.Kill();
            fullMapSequence = null;
        }
    }

    // 맵모드 전환
    public void ChangeMapMode()
    {
        // 시퀀스 진행 중 중복 실행 방지
        if (fullMapSequence != null && fullMapSequence.IsPlaying()) return;

        if (currentMapMode == MapMode.MiniMap)
        {
            SetFullMapMode();
        }
        else
        {
            SetMiniMapMode();
        }
    }

    // 미니맵 모드로 전환하는 로직
    private void SetMiniMapMode()
    {
        currentMapMode = MapMode.MiniMap;

        // 앵커 변경
        miniMapRectTransform.anchorMin = anchorMinMiniMap;
        miniMapRectTransform.anchorMax = anchorMaxMiniMap;

        // 정렬 순서 변경
        minimapCanvas.sortingOrder = firstSortingOrder;

        // 시퀀스 역재생 (스케일 및 위치 애니메이션)
        fullMapSequence.PlayBackwards();

        // 카메라 줌 복원
        if (minimapCamera != null)
        {
            minimapCamera.DOOrthoSize(miniMapCameraSize, animationDuration);
        }
    }

    // 전체 맵 모드로 전환하는 로직
    private void SetFullMapMode()
    {
        currentMapMode = MapMode.FullMap;

        // 앵커 변경
        miniMapRectTransform.anchorMin = anchorMinFullMap;
        miniMapRectTransform.anchorMax = anchorMaxFullMap;

        // 정렬 순서 변경
        minimapCanvas.sortingOrder = fullMapSortingOrder;

        // 시퀀스 실행 (스케일 및 위치 애니메이션)
        fullMapSequence.Restart();

        // 카메라 줌 확대
        if (minimapCamera != null)
        {
            minimapCamera.DOOrthoSize(fullMapCameraSize, animationDuration);
        }
    }
}
