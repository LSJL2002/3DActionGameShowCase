using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// 제네릭 싱글톤 스크립트를 상속
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    // 현재 매니저가 존재하는지 확인용
    public bool isManager;

    // 현재씬의 번호를 저장할 변수
    public int NowSceneIndex;

    protected override void Awake()
    {
        base.Awake();

        // 변수에 현재 열려있는 씬의 번호를 가져와 저장
        NowSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // callback : 실행할 함수를 매개변수로 받음
    // 로딩완료 후 할 Task가 없을 수도 있으니 기본 null 설정
    // 기본 로드씬모드를 싱글로 설정 (싱글은 단일 로드, Additive는 비동기 로드)
    public async void ChangeScene(int SceneIndex, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var op = SceneManager.LoadSceneAsync(SceneIndex, loadSceneMode);

        while (!op.isDone)
        {
            // 로딩UI 활성화
            Debug.Log("로딩중");

            await Task.Yield();
        }

        Debug.Log("로딩 완료");

        // 싱글로 로드했다면
        if (loadSceneMode == LoadSceneMode.Single)
        {
            NowSceneIndex = SceneIndex;
        }

        // 콜백이 있다면 실행
        callback?.Invoke();
    }
    
    // 씬 언로드 함수
    public async void UnLoadScene(string SceneName, Action callback = null)
    {
        var op = SceneManager.UnloadSceneAsync(SceneName);

        while (!op.isDone)
        {
            Debug.Log("씬 언로드");
            await Task.Yield();
        }

        callback?.Invoke();
    }
}