using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
        if (!assetPool.ContainsKey(key + "_" + typeStr))
        {
            Debug.Log(key + " 새로 생성");
            
            // 리소스폴더의 경로와 T타입을 지정하여 리소소를 비동기 로드시작 (오브젝트를 생성하는건 아니고 로드만)
            var op = Resources.LoadAsync(typeStr, typeof(T));

            // 비동기 로드가 끝나지 않았다면 반복
            while(!op.isDone)
            {
                Debug.Log(key + "로드중");

                // 현재 실행 중인 코드를 일시 중단하고 다음 프레임에 다시 실행을 재개
                await Task.Yield();
            }

            // asset : 로드된 리소스 객체 자체를 뜻함
            var obj = op.asset;

            if (obj == null)
            {
                return default;
            }

            // 딕셔너리에 추가 (key, value)
            assetPool.Add(key + "_" + typeStr, obj);
        }

        // 특정 key의 값을 handle 변수에 저장
        Debug.Log(key + " 반환");
        handle = (T)assetPool[key + "_" + typeStr];

        // handle을 반환
        return handle;
    }
}

