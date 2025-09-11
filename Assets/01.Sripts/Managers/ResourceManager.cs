using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// 리소스폴더 1차 경로
public enum eAssetType
{
    Prefab,
    UI,
}

// 리소스폴더 2차 경로
public enum eCategoryType
{
    none,
    item,
    stage,
}

// 제네릭 싱글톤 스크립트를 상속
public class ResourceManager : Singleton<ResourceManager>
{
    // 딕셔너리 생성(string 형식의 key, object 형식의 value)
    private Dictionary<string, object> assetPool = new Dictionary<string, object>();

    // 반환타입 : Task<T> (비동기 메서드)
    // 매개변수 : key 이름, 1차경로, 2차경로
    // 2차경로는 없을시 기본 none 적용
    public async Task<T> LoadAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.none)
    {
        T handle = default;

        // 리소스폴더의 경로를 변수로 만들어 저장
        var typeStr = $"{assetType}{(categoryType == eCategoryType.none ? "" : $"/{categoryType}")}/{key}";

        // 딕셔너리에 없다면
        if (!assetPool.ContainsKey(key))
        {            
            // 리소스폴더의 경로와 T타입을 지정하여 리소소를 비동기 로드시작 (오브젝트를 생성하는건 아니고 로드만)
            var op = Resources.LoadAsync(typeStr, typeof(T));

            // 비동기 로드가 완료될때까지 대기
            await op;

            // asset : 로드된 리소스 객체 자체를 뜻함
            var obj = op.asset;

            if (obj == null)
            {
                return default;
            }

            // 딕셔너리에 추가 (key, value)
            assetPool.Add(key, obj);
        }

        // 특정 key의 값을 handle 변수에 저장
        handle = (T)assetPool[key];

        // handle을 반환
        return handle;
    }

    // 다른 스크립트에서 생성한 리소스를 쉽게 가져올 수 있도록 제네릭 메서드 제공
    public T Get<T>(string key) where T : Object
    {
        if (assetPool.TryGetValue(key, out object obj))
        {
            return (T)obj;
        }

        Debug.LogError($"에셋 '{key}'이 없음");
        return default;
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
        // 딕셔너리 '참조'만 삭제
        assetPool.Clear();

        // 어디에도 참조되지 않은 에셋들을 찾아 메모리에서 완전히 언로드하는 함수 호출
        Resources.UnloadUnusedAssets();
    }
}

