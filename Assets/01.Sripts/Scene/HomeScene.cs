using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HomeScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();

        LoadingManager.Instance.SetLoadingPanel(true); // 로딩 UI 켜기
    }

    protected async override void Start()
    {
        base.Start();

        GameObject homeObject = await LoadAddress("HomeObject"); // 홈 씬 오브젝트 로드
        LoadingManager.Instance.SetLoadingPanel(false); // 로딩 UI 끄기
        OnHomeSceneStart(); // 홈 씬 시작
    }

    public async void OnHomeSceneStart()
    {
        await UIManager.Instance.Show<TitleUI>();
        AudioManager.Instance.PlayBGM("2");
        Cursor.lockState = CursorLockMode.None;
    }

    public async UniTask<GameObject> LoadAddress(string objectAddress)
    {
        var newHandle = Addressables.InstantiateAsync(objectAddress);
        await newHandle.ToUniTask();
        return newHandle.Result;
    }
}
