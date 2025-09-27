using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 제네릭 싱글톤 스크립트를 상속
public class UIManager : Singleton<UIManager>
{
    public static int ScreenWidth = 1920;
    public static int ScreenHeight = 1080;

    public UIBase previousUI; // 이전 활성화 된 UI를 저장할 변수
    public UIBase currentUI; // 현재 활성화 된 UI를 저장할 변수

    // 한번 생성한 UI를 다시 생성하지 않도록 Dictionary로 관리
    private Dictionary<string, AsyncOperationHandle<GameObject>> uiHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();

    // UI호출시 사용하는 함수
    // 리소스매니저의 LoadAsset 메서드가 비동기 메서드이므로 Show 메서드도 비동기 메서드로 변경 (반환타입 async Task<T>)
    public async UniTask<T> Show<T>() where T : UIBase
    {
        // 현재UI상태가 있었다면, 이전UI상태 변수에 저장
        if (currentUI != null)
        {
            previousUI = currentUI;
        }

        string uiName = typeof(T).ToString();

        // 딕셔너리에 있는지 확인
        // TryGetValue : 딕셔너리에서 값을 가져오는것을 시도 (반환값이 bool이기 때문에 if에 사용가능)
        // IsValid() : 어드레서블 핸들이 유효한 상태인지 확인하는 함수 (메모리에 있는지)
        if (uiHandles.TryGetValue(uiName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            T uiBase = handle.Result.GetComponent<T>();
            uiBase.canvas.gameObject.SetActive(true);
            currentUI = uiBase;
            return uiBase;
        }

        // 없으면 새로 로드
        else
        {
            T uiBase = await Load<T>(uiName);
            currentUI = uiBase;
            return uiBase;
        }
    }

    // UniTask<T> : 비동기메서드에서 특정타입(T)의 반환값이 있을경우 사용 (C# 기본 Task 상위호환)
    public async UniTask<T> Load<T>(string uiName) where T : UIBase
    {
        // 캔버스 오브젝트 생성
        var newCanvasObject = new GameObject(uiName + "Canvas");

        // 'Canvas' 컴포넌트 추가 후 변수에 저장 및 renderMode 설정
        var canvas = newCanvasObject.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 'CanvasScaler' 컴포넌트 추가 후 변수에 저장 및 uiScaleMode, referenceResolution 설정
        var canvasScaler = newCanvasObject.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);

        // 'GraphicRaycaster' 컴포넌트 추가
        newCanvasObject.gameObject.AddComponent<GraphicRaycaster>();

        // 어드레서블 로드 및 딕셔너리에 추가
        var newHandle = Addressables.InstantiateAsync(uiName, newCanvasObject.transform);
        var obj = await newHandle.Task;
        obj.name = uiName;
        uiHandles.Add(uiName, newHandle);

        var result = obj.GetComponent<T>();
        result.canvas = canvas;
        result.canvas.sortingOrder = uiHandles.Count;

        return result;
    }

    // 다른 스크립트에서 UI를 쉽게 가져올 수 있도록 제네릭 메서드 제공
    public T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();

        // 핸들을 통해 UIBase 인스턴스를 반환
        if (uiHandles.TryGetValue(uiName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            return handle.Result.GetComponent<T>();
        }

        Debug.LogError($"에셋 '{uiName}'이 없음");
        return default;
    }

    // UI를 숨길 때 호출
    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();

        if (uiHandles.TryGetValue(uiName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            handle.Result.GetComponent<UIBase>().canvas.gameObject.SetActive(false);
        }
    }

    public void Hide(string uiName)
    {
        if (uiHandles.TryGetValue(uiName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            handle.Result.GetComponent<UIBase>().canvas.gameObject.SetActive(false);
        }
    }

    // 지정된 UI 빼고 딕셔너리 목록 전부 Hide 하는 함수
    public void AllHide(UIBase uiName = null)
    {
        foreach (var handle in uiHandles.Values)
        {
            // 핸들이 유효하고 결과 오브젝트가 있는지 확인
            if (handle.IsValid() && handle.Result != null)
            {
                UIBase uiBase = handle.Result.GetComponent<UIBase>();

                // 현재 UI(currentUI)와 다르다면 비활성화
                if (uiBase != uiName)
                {
                    uiBase.canvas.gameObject.SetActive(false);
                }
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // 씬 언로드 이벤트 구독
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // 씬 언로드 이벤트 구독 해제
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // 이전 씬에서 사용한 딕셔너리 리스트 정리 (씬 언로드시 호출할것)
    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "PlayerUIScene")
            return;

        foreach (var handle in uiHandles.Values)
        {
            if (handle.IsValid())
            {
                Addressables.ReleaseInstance(handle);
            }
        }

        uiHandles.Clear();
    }
}
