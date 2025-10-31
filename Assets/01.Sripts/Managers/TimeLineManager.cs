using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;

// 해당 씬에만 존재할 타임라인 매니저
public class TimeLineManager : Singleton<TimeLineManager>
{
    // 한번 생성한 핸들을 다시 생성하지 않도록 Dictionary로 관리
    private Dictionary<string, AsyncOperationHandle<GameObject>> timelineHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();

    protected async override void Start()
    {
        await Task.Yield();
        base.Start();

        PlayerManager.Instance.OnAllCharactersDead += OnGameOverTimeLine; // 플레이어 사망이벤트에 구독
    }

    // 게임오버 타임라인 호출하는 함수
    private async void OnGameOverTimeLine()
    {
        PlayerManager.Instance.OnAllCharactersDead -= OnGameOverTimeLine; // 플레이어 사망이벤트에 구독해제

        await OnTimeLine<PlayableDirector>("TimeLine_GameOver");
    }

    public async UniTask<T> OnTimeLine<T>(string addressableKey) where T : PlayableDirector
    {
        string timelineName = addressableKey;

        // 딕셔너리에 있는지 확인
        // TryGetValue : 딕셔너리에서 값을 가져오는것을 시도 (반환값이 bool이기 때문에 if에 사용가능)
        // IsValid() : 어드레서블 핸들이 유효한 상태인지 확인하는 함수 (메모리에 있는지)
        if (timelineHandles.TryGetValue(timelineName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            T timeline = handle.Result.GetComponent<T>();
            timeline.gameObject.SetActive(!timeline.gameObject.activeSelf);
            return timeline;
        }

        // 없으면 새로 로드
        else
        {
            T timeline = await Load<T>(timelineName);
            return timeline;
        }
    }

    public async UniTask<T> Load<T>(string timelineName) where T : PlayableDirector
    {
        var newHandle = Addressables.InstantiateAsync(timelineName);
        var obj = await newHandle.Task;
        obj.name = timelineName;
        if (timelineHandles.ContainsKey(timelineName)) return null;
        timelineHandles.Add(timelineName, newHandle);
        var result = obj.GetComponent<T>();
        return result;
    }

    // 다른 클래스에서 쉽게 가져갈 수 있도록 제네릭 메서드 제공
    public T Get<T>() where T : PlayableDirector
    {
        string timelineName = typeof(T).ToString();

        // 핸들을 통해 UIBase 인스턴스를 반환
        if (timelineHandles.TryGetValue(timelineName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            return handle.Result.GetComponent<T>();
        }

        Debug.LogError($"'{timelineName}'이 없음");
        return default;
    }

    // 타임라인을 숨길 때 호출
    public void Hide<T>() where T : PlayableDirector
    {
        string timelineName = typeof(T).ToString();

        if (timelineHandles.TryGetValue(timelineName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            handle.Result.GetComponent<PlayableDirector>().gameObject.SetActive(false);
        }
    }

    public void Hide(string timelineName)
    {
        if (timelineHandles.TryGetValue(timelineName, out AsyncOperationHandle<GameObject> handle) && handle.IsValid())
        {
            handle.Result.GetComponent<PlayableDirector>().gameObject.SetActive(false);
        }
    }

    // 타임라인을 릴리즈 할 때 호출
    public void Release<T>() where T : PlayableDirector
    {
        string timelineName = typeof(T).ToString();

        if (timelineHandles.TryGetValue(timelineName, out var handle))
        {
            if (handle.IsValid())
            {
                Addressables.ReleaseInstance(handle);
            }
            //else
            //{
            //    Debug.LogWarning($"'{timelineName}' 핸들은 유효하지 않음");
            //}
            // 딕셔너리에서도 제거
            timelineHandles.Remove(timelineName);
        }
        //else
        //{
        //    Debug.LogWarning($"'{timelineName}'가 딕셔너리에 없음");
        //}
    }

    public void Release(string timelineName)
    {
        if (timelineHandles.TryGetValue(timelineName, out var handle))
        {
            if (handle.IsValid())
            {
                Addressables.ReleaseInstance(handle);
            }
            //else
            //{
            //    Debug.LogWarning($"'{timelineName}' 핸들은 유효하지 않음");
            //}
            // 딕셔너리에서도 제거
            timelineHandles.Remove(timelineName);
        }
        //else
        //{
        //    Debug.LogWarning($"'{timelineName}'가 딕셔너리에 없음");
        //}
    }

    public async UniTask PlayAndWait(string timelineName)
    {

        // 1. 타임라인이 없거나 핸들이 유효하지 않으면 자동 로드
        if (!timelineHandles.TryGetValue(timelineName, out var handle) || !handle.IsValid())
        {
            await Load<PlayableDirector>(timelineName);
            handle = timelineHandles[timelineName];
        }

        // 2. PlayableDirector 가져오기
        var director = handle.Result.GetComponent<PlayableDirector>();
        if (director == null)
        {
            return;
        }

        var tcs = new UniTaskCompletionSource();

        // 3. 종료 이벤트 연결
        void OnStopped(PlayableDirector d)
        {
            director.stopped -= OnStopped;
            tcs.TrySetResult();
        }

        director.stopped += OnStopped;

        // 4. 타임라인 실행
        director.gameObject.SetActive(true);
        director.Play();

        // 5. 종료/스킵까지 대기
        await tcs.Task;

    }


}


