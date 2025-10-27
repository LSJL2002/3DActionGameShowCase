using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// 제네릭 싱글톤 스크립트를 상속
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    // 매니저들이 이미 초기화 됐는지 확인하는 변수
    public bool isManager;

    // 현재씬의 번호를 저장할 변수
    public int nowSceneIndex;

    // ChangeScene으로 받은 씬번호를 저장할 변수
    public int targetSceneIndex;

    protected override void Awake()
    {
        base.Awake();

        // 변수에 현재 열려있는 씬의 번호를 가져와 저장
        nowSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // 매개변수1(씬번호) : 로드할 씬번호
    // 매개변수2(함수) : 함수 마지막에 실행할 함수를 매개변수로 받음
    // 매개변수3(씬로드방식) : Single은 동기 로드, Additive는 비동기 로드
    public async void ChangeScene(int sceneIndex, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        // 빌드에 등록된 씬 개수보다 인덱스가 크거나 0보다 작으면 오류 로그를 표시
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"{sceneIndex} 없음");
            return; // 함수 실행 종료
        }

        var op = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);

        // 로드 완료까지 대기
        await op;

        // 싱글로 로드했다면
        if (loadSceneMode == LoadSceneMode.Single)
        {
            nowSceneIndex = sceneIndex;
        }

        // 콜백이 있다면 실행
        callback?.Invoke();
    }
    
    // 씬 언로드 함수
    public async void UnLoadScene(int SceneIndex, Action callback = null)
    {
        var op = SceneManager.UnloadSceneAsync(SceneIndex);

        // 로드 완료까지 대기
        await op;

        callback?.Invoke();
    }
}