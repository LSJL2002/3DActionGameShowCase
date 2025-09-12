using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private Dictionary<string, UIBase> ui_List = new Dictionary<string, UIBase>();

    // 다른 스크립트에서 UI를 쉽게 띄울 수 있도록 제네릭 메서드 제공
    // 리소스매니저의 LoadAsset 메서드가 비동기 메서드이므로 Show 메서드도 비동기 메서드로 변경 (반환타입 async Task<T>)
    public async Task<T> Show<T>() where T : UIBase
    {
        // 현재UI상태가 있었다면, 이전UI상태 변수에 저장
        if (currentUI != null)
        {
            previousUI = currentUI;
        }

        string uiName = typeof(T).ToString();

        // 딕셔너리에서 key : uiName에 해당하는 UIBase를 꺼내서 uiBase에 저장
        ui_List.TryGetValue(uiName, out UIBase uiBase);

        if (uiBase == null)
        {
            // 없으면 로드 함수를 통해서 리소스매니저의 함수를 호출하여 UI와 캔버스를 로드
            // await : 반환타입이 string이 되도록 멈췄다가 받고 변수에 저장한다는 의미
            // (await이 없으면 Task<T> 타입이 되어버려서 타입불일치 오류)
            uiBase = await Load<T>(uiName);

            // 생성한 리소스를 딕셔너리에 추가
            ui_List.Add(uiName, uiBase);

            // 해당 UI 활성화
            uiBase.canvas.gameObject.SetActive(true);
        }
        else
        {
            // 해당 UI 활성화 <-> 비활성화
            uiBase.canvas.gameObject.SetActive(!uiBase.canvas.gameObject.activeSelf);
        }

        // 현재 UI상태를 변수에 저장
        currentUI = uiBase;

        return (T)uiBase;
    }

    // 리소스매니저를 통하여 UI 프리팹을 로드하고 캔버스를 생성하는 메서드
    // 리소스매니저의 LoadAsset 메서드가 비동기 메서드이므로 Show 메서드도 비동기 메서드로 변경 (반환타입 async Task<T>)
    public async Task<T> Load<T>(string uiName) where T : UIBase
    {
        // ""으로 된 새로운 게임오브젝트 생성
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

        // 리소스매니저의 LoadAsset 메서드를 호출하여 UI 프리팹을 로드 후 변수에 저장
        // await : 반환타입이 GameObject가 되도록 멈췄다가 받고 변수에 저장한다는 의미
        // (await이 없으면 Task<T> 타입이 되어버려서 타입불일치 오류)
        var prefab = await ResourceManager.Instance.LoadAsset<GameObject>(uiName, eAssetType.UI);

        // 프리팹을 캔버스의 자식으로 생성 후 변수에 저장
        var obj = Instantiate(prefab, newCanvasObject.transform);

        // Instantiate를 통해 생성된 오브젝트의 이름에서 (Clone) 제거
        obj.name = obj.name.Replace("(Clone)", "");

        var result = obj.GetComponent<T>();
        result.canvas = canvas;
        result.canvas.sortingOrder = ui_List.Count;

        return result;
    }

    // 다른 스크립트에서 UI를 쉽게 가져올 수 있도록 제네릭 메서드 제공
    public T Get<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();
        

        if(ui_List.TryGetValue(uiName, out UIBase uiBase))
        {
            return (T)uiBase;
        }

        Debug.LogError($"에셋 '{uiName}'이 없음");
        return default;
    }

    // UI를 숨길 때 호출
    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();

        Hide(uiName);
    }

    public void Hide(string uiName)
    {
        ui_List.TryGetValue(uiName, out UIBase uiBase);
        
        if(uiBase == null)
        {
            return;
        }

        // 생성한 UI 비활성화
        uiBase.canvas.gameObject.SetActive(false);

        // UI 제거 (딕셔너리에서 삭제하고 캔버스 오브젝트 파괴)
        // DestroyImmediate(uiBase.canvas.gameObject);
        // ui_List.Remove(uiName);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // 씬 로드 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 이전 씬에서 사용한 딕셔너리 리스트 정리 (씬전환시 호출할것)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var uiBase in ui_List.Values)
        {
            if (uiBase != null && uiBase.canvas != null)
            {
                Destroy(uiBase.canvas.gameObject);
            }
        }

        // 딕셔너리 '참조'만 삭제
        ui_List.Clear();

        // 어디에도 참조되지 않은 에셋들을 찾아 메모리에서 완전히 언로드하는 함수 호출
        Resources.UnloadUnusedAssets();
    }
}
