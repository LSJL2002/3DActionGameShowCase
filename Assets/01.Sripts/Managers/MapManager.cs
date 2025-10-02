using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : Singleton<MapManager>
{
    private Dictionary<int, BattleZone> zoneDict = new Dictionary<int, BattleZone>();
    public BattleZone currentZone;

    [SerializeField] private int startingZoneId;
    [SerializeField] private int BossZoneId;

    [SerializeField] private int round; // 게임매니저로..

    [SerializeField] private GameObject tutorialWall;

    private void OnEnable()
    {
        BattleManager.OnBattleStart += OpenZone;
        BattleManager.OnBattleClear += OpenNextZone;
        TutorialUI.endTutorial += tutorialWallToggle;
    }

    private void OnDisable()
    {
        BattleManager.OnBattleStart -= OpenZone;
        BattleManager.OnBattleClear -= OpenNextZone;
        TutorialUI.endTutorial -= tutorialWallToggle;
    }

    private async void Start()
    {
        //GameObject map = await LoadAscync("Map");
        //if (map != null)
        //{
        //    Debug.Log("Map 성공적으로 불러옴!");
        //}
        //else
        //{
        //    Debug.Log("Map 로드실패");
        //}

        //GameObject btZone = await LoadAscync("BattleZone");
        //if (btZone != null)
        //{
        //    Debug.Log("BattleZone 성공적으로 불러옴!");
        //}
        //else
        //{
        //    Debug.LogError("BattleZone 로드 실패");
        //}

        //// NavMeshData만 로드 (맵 위치 기준으로 맞출 수 있음)
        //await LoadNavMesh("NavMesh", Vector3.zero, Quaternion.identity);
        ////생성
        ////AddressableManager.Instance.MakeGameObject("Map");


        //배틀존 딕셔너리에 아이디를 키값으로 등록
        ResetZones();
    }
    public void ResetZones()
    {
        zoneDict.Clear(); // 이전 존 정보 싹 비우기

        var zones = FindObjectsOfType<BattleZone>();
        foreach (var zone in zones)
        {
            RegisterStage(zone);
            zone.SetWallsActive(false);
            zone.gameObject.SetActive(false);
        }

        if (zoneDict.TryGetValue(startingZoneId, out var startZone))
        {
            startZone.gameObject.SetActive(true);
            currentZone = startZone;
        }

       if(tutorialWall != null)
        {
            tutorialWall = GameObject.Find("TutorialWall");
            tutorialWall.SetActive(true);
        }
        Debug.Log("맵 초기화 완료");
    }

    public void tutorialWallToggle()
    {
        if(tutorialWall != null)
        tutorialWall.SetActive(false);
    }

    public void RegisterStage(BattleZone zone)
    {
        if (!zoneDict.ContainsKey(zone.id)) //아이디가없으면 추가
            zoneDict.Add(zone.id, zone);
    }

    private void OpenZone(BattleZone zone) //입장시
    {
        currentZone = zone;

        foreach (var otherZone in zoneDict)  //나머지 스테이지 꺼줌
        {
            if (otherZone.Value != zone)
            {
                otherZone.Value.gameObject.SetActive(false);
            }
        }
    }

    private void OpenNextZone(BattleZone zone) // 클리어시
    {
        if (zone.moveAbleStage == null || zone.moveAbleStage.Count == 0)
        {
            SceneLoadManager.Instance.LoadScene(0);
            Debug.Log("마지막 스테이지 클리어!");
            round++;
            Debug.Log("현재 회차 : " + round);
            //if (round == 0)
            // SceneManager.LoadScene("Stage2");
            //if (round == 1)
            //진엔딩
            return;
        }

        foreach (var nextId in zone.moveAbleStage)
        {
            if (zoneDict.TryGetValue(nextId, out var nextZone))
            {
                nextZone.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("다음 스테이지 없음 → 마지막 스테이지거나 잘못된 ID");
            }
        }

        //클리어한 존 지우기
        zone.gameObject.SetActive(false); //Release
        //zoneDict.Remove(zone.id);
        //Addressables.ReleaseInstance(zone.gameObject);
        currentZone = null;

    }

    //public async Task<GameObject> LoadAscync(string str)
    //{   // 프리팹 비동기 로드 & 인스턴스화
    //    var op = Addressables.InstantiateAsync(str, Vector3.zero, Quaternion.identity);
    //    await op.Task; // 여기서 먼저 대기
    //    Debug.Log(op.PercentComplete);
    //    if (op.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        return op.Result; // 성공적으로 불러온 프리팹 반환
    //    }
    //    else
    //    {
    //        Debug.LogError($"BattleZone 로드 실패: {op}");
    //        return null; // 실패 시 null 반환
    //    }

    //}



    //public async Task LoadNavMesh(string navKey, Vector3 position, Quaternion rotation)
    //{
    //    // 1. NavMeshData 불러오기
    //    var navOp = Addressables.LoadAssetAsync<NavMeshData>(navKey);
    //    NavMeshData navData = await navOp.Task;

    //    if (navData == null)
    //    {
    //        Debug.LogError($"NavMeshData 로드 실패: {navKey}");
    //        return;
    //    }

    //    // 2. NavMeshData 등록
    //    navMeshInstance = NavMesh.AddNavMeshData(navData, position, rotation);

    //    Debug.Log($"NavMeshData 로드 완료: {navKey}");
    //}

    //public void UnloadNavMesh()
    //{
    //    navMeshInstance.Remove();
    //    Debug.Log("NavMeshData 제거 완료");
    //}
}

//[Header("Stage Flow")]
//public int startingStageID;

//private Dictionary<int, BattleZone> stageDict = new();
//private BattleZone currentZone;
////private BattleZone lastClearedZone;

//void OnEnable()
//{
//    BattleZone.OnBattle += HandleZoneEnter;
//    BattleZone.OnBattleClear += HandleZoneClear;
//}

//void OnDisable()
//{
//    BattleZone.OnBattle -= HandleZoneEnter;
//    BattleZone.OnBattleClear -= HandleZoneClear;
//}

//void Start()
//{
//    // 씬에 있는 모든 BattleZone 등록
//foreach (var zone in FindObjectsOfType<BattleZone>(true))
//{
//    RegisterStage(zone);
//    zone.Deactivate();
//}

//// 시작 스테이지만 켜기
//if (stageDict.TryGetValue(startingStageID, out var startZone))
//{
//    startZone.Activate();
//}
//}

//public void RegisterStage(BattleZone zone)
//{
//    if (!stageDict.ContainsKey(zone.stageID))
//        stageDict.Add(zone.stageID, zone);
//}

//private void HandleZoneEnter(BattleZone zone)
//{
//    currentZone = zone;
//    zone.StartBattle();

//    foreach (var kvp in stageDict)
//    {
//        if (kvp.Value != currentZone)
//        {
//            kvp.Value.Deactivate();
//            Debug.Log($"{kvp.Value.stageID} 번 Zone 비활성화");
//        }
//    }

//}

//private void HandleZoneClear(BattleZone zone)
//{
//    if (!stageDict.ContainsKey(zone.stageID)) return;
//    zone._monster.SetActive(false);
//    // 다음 스테이지 열기
//    foreach (var id in zone.nextStages)
//    {
//        var next = GetStage(id);
//        if (next != null) next.Activate();
//    }

//    if (zone.isEndingStage)
//    {
//        Debug.Log("모든 스테이지 클리어! 엔딩씬으로 이동!");
//        //SceneLoadManager.Instance.LoadScene(04);
//    }

//    //lastClearedZone = zone;
//    currentZone = null;
//}

//public BattleZone GetStage(int id)
//{
//    stageDict.TryGetValue(id, out var zone);
//    return zone;
//}
