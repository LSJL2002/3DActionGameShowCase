using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    private Dictionary<int, BattleZone> zoneDict = new Dictionary<int, BattleZone>();
    public BattleZone currentZone;

    [SerializeField] private int startingZoneId; //처음 켜줄 Zone 아이디
    [SerializeField] private int _bossZoneId = 60000010; //마지막 Zone 아이디
    public int bossZoneId => _bossZoneId;

    [SerializeField] private int round;  // 회차

    [SerializeField] private GameObject tutorialWall;

    NavMeshDataInstance navMeshInstance;

    protected override void Start()
    {
        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleStart, OpenZone); // 구독해제
        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleClear, OpenNextZone); // 구독해제
        TutorialUI.endTutorial -= tutorialWallToggle; // 구독해제

        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleStart, OpenZone); // 구독
        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleClear, OpenNextZone); // 구독
        TutorialUI.endTutorial += tutorialWallToggle; // 구독
    }

    public async Task LoadMap()
    {
        GameObject map = await LoadAscync("StatgeSceneMap");
        if (map != null)
        {
            Debug.Log("Map 성공적으로 불러옴!");
        }
        else
        {
            Debug.Log("Map 로드실패");
        }

        GameObject btZone = await LoadAscync("BattleZone");
        if (btZone != null)
        {
            Debug.Log("BattleZone 성공적으로 불러옴!");
        }
        else
        {
            Debug.LogError("BattleZone 로드 실패");
        }
        // NavMeshData만 로드 (맵 위치 기준으로 맞출 수 있음)
        await LoadNavMesh("NavMesh", Vector3.zero, Quaternion.identity);
    }

    public void ResetZones()
    {
        var controller = PlayerManager.Instance.ActiveCharacter.GetComponent<CharacterController>();
        zoneDict.Clear(); // 이전 존 정보 싹 비우기

        var zones = Object.FindObjectsByType<BattleZone>(FindObjectsSortMode.None);
        foreach (var zone in zones)
        {
            RegisterStage(zone);
            zone.SetWallsActive(false);
            zone.gameObject.SetActive(false);
        }
        if (tutorialWall == null)
        {
            tutorialWall = GameObject.Find("TutorialWall");
        }

        if (SaveManager.Instance.gameMode != eGameMode.LoadGame)           //불러오기가 아닐때
        {
            round = SaveManager.Instance.playerData.round;
            tutorialWall.SetActive(true);
            ReturnToStartZone();
            UIManager.Instance.tutorialEnabled = true;

            if (controller != null)
            {
                controller.enabled = false; // 움직임 제어 잠깐 끄기
                PlayerManager.Instance.ActiveCharacter.transform.position = new Vector3(0,0,5);
                controller.enabled = true;  // 다시 켜기
            }
        }

        else if (SaveManager.Instance.gameMode == eGameMode.LoadGame)
        {
            
            tutorialWall.SetActive(false);
            int lastClearStage = SaveManager.Instance.playerData.LastClearStage;

            
            if (lastClearStage == _bossZoneId)
            {
                ReturnToStartZone();
            }
            else if (zoneDict.TryGetValue(lastClearStage, out var clearedZone))
            {
                
                if (controller != null)
                {
                    controller.enabled = false; // 움직임 제어 잠깐 끄기
                    PlayerManager.Instance.ActiveCharacter.transform.position = clearedZone.transform.position;
                    controller.enabled = true;  // 다시 켜기
                }
                OpenNextZone(clearedZone);
                //await Task.Yield();

            }


            currentZone = null;
            Debug.Log("맵 불러오기 완료");
        }
    }

    private void ReturnToStartZone()
    {
        if (zoneDict.TryGetValue(startingZoneId, out var startZone))
        {
            // 모든 존 비활성화 (안전장치)
            foreach (var zone in zoneDict.Values)
                zone.gameObject.SetActive(false);

            startZone.gameObject.SetActive(true);
            currentZone = startZone;
            Debug.Log("시작 스테이지로 복귀!");
        }
        else
        {
            Debug.LogError("시작 스테이지를 찾을 수 없습니다!");
        }
    }

    public void tutorialWallToggle()
    {
        if (tutorialWall != null)
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
            return;

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

        //클리어한 존 비활성화
        zone.gameObject.SetActive(false); //Release
        //zoneDict.Remove(zone.id);
        //Addressables.ReleaseInstance(zone.gameObject);
        currentZone = null;

    }

    public void HandleLastStageClear()
    {
        SaveManager.Instance.playerData.round++;
        Debug.Log($"마지막 스테이지 클리어! 현재 회차: {round}");

        // 필요시 세이브 추가
        SaveManager.Instance.playerData.LastClearStage = 0;
        SaveManager.Instance.SaveData();       

        // 씬 리셋 or 엔딩씬
        SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
    }

    public async Task<GameObject> LoadAscync(string str)
    {   // 프리팹 비동기 로드 & 인스턴스화
        var op = Addressables.InstantiateAsync(str, Vector3.zero, Quaternion.identity);
        await op.Task; // 여기서 먼저 대기
        Debug.Log(op.PercentComplete);
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            return op.Result; // 성공적으로 불러온 프리팹 반환
        }
        else
        {
            Debug.LogError($"BattleZone 로드 실패: {op}");
            return null; // 실패 시 null 반환
        }

    }



    public async Task LoadNavMesh(string navKey, Vector3 position, Quaternion rotation)
    {
        // 1. NavMeshData 불러오기
        var navOp = Addressables.LoadAssetAsync<NavMeshData>(navKey);
        NavMeshData navData = await navOp.Task;

        if (navData == null)
        {
            Debug.LogError($"NavMeshData 로드 실패: {navKey}");
            return;
        }

        // 2. NavMeshData 등록
        navMeshInstance = NavMesh.AddNavMeshData(navData, position, rotation);

        Debug.Log($"NavMeshData 로드 완료: {navKey}");
    }

    public void UnloadNavMesh()
    {
        navMeshInstance.Remove();
        Debug.Log("NavMeshData 제거 완료");
    }
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


// new Game  - Reset해주면됨 / 다지워짐
// Load Game  - 로드해야됨 / 하려면 저장데이터가 필요함. 
// 스테이지에서 필요한 저장데이터는 마지막으로 클리어한 스테이지 LastClearedStage(Zone)변수에 저장할건데 StageID를 저장할것임
// 몇회차인지도 저장, 불러오기가필요함
// 이 데이터는 뉴게임을 누르지않는이상 초기화 될필요가없음
// 불러왔다면 해당 배틀존을 클리어한 시점이 되어야하며, 플레이어의 위치는 그 배틀존 혹은 zone.transform.position+(0,0,100)
