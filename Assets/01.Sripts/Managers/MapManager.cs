using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapManager : Singleton<MapManager>
{
    private Dictionary<int, BattleZone> zoneDict = new Dictionary<int, BattleZone>();
    public BattleZone currentZone;

    [SerializeField] public int startingZoneId; //처음 켜줄 Zone 아이디
    [SerializeField] private int _bossZoneId = 60000010; //마지막 Zone 아이디
    public int bossZoneId => _bossZoneId;

    [SerializeField] private int round;  // 회차

    [SerializeField] private GameObject tutorialWall;
    [SerializeField] public GameObject gameClearDoor;

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


        GameObject btZone = await LoadAscync("BattleZone");

        // NavMeshData만 로드 (맵 위치 기준으로 맞출 수 있음)
        await LoadNavMesh("NavMesh", Vector3.zero, Quaternion.identity);

        GetComponent<SkyboxBlendController>()?.skyInitialize();

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
        if (gameClearDoor == null)
        {
            gameClearDoor = FindAnyObjectByType<GameClearDoor>().gameObject;
            gameClearDoor.SetActive(false);
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
                PlayerManager.Instance.ActiveCharacter.transform.position = new Vector3(0, 0, 5);
                controller.enabled = true;  // 다시 켜기
            }
        }

        else if (SaveManager.Instance.gameMode == eGameMode.LoadGame)
        {

            tutorialWall.SetActive(false);
            int lastClearStage = SaveManager.Instance.playerData.LastClearStage;


            if (lastClearStage == _bossZoneId || lastClearStage == 0)
            {
                ReturnToStartZone();
                controller.enabled = false; // 움직임 제어 잠깐 끄기
                PlayerManager.Instance.ActiveCharacter.transform.position = new Vector3(0, 0, 5);
                controller.enabled = true;  // 다시 켜기
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


            }


            currentZone = null;

        }

    }

    public void ReturnToStartZone()
    {
        if (zoneDict.TryGetValue(startingZoneId, out var startZone))
        {
            // 모든 존 비활성화 (안전장치)
            foreach (var zone in zoneDict.Values)
                zone.gameObject.SetActive(false);

            startZone.gameObject.SetActive(true);
            currentZone = startZone;

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

        }
        zone.gameObject.SetActive(false);
        currentZone = null;
    }

    public void HandleLastStageClear()
    {
        SaveManager.Instance.playerData.round++;


        // 세이브
        SaveManager.Instance.playerData.LastClearStage = 0;
        SaveManager.Instance.SaveData();

        //클리어 문 생성
        gameClearDoor.SetActive(true);
    }

    public async Task<GameObject> LoadAscync(string str)
    {   // 프리팹 비동기 로드 & 인스턴스화
        var op = Addressables.InstantiateAsync(str, Vector3.zero, Quaternion.identity);
        await op.Task; // 여기서 먼저 대기
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            return op.Result; // 성공적으로 불러온 프리팹 반환
        }
        else
        {

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
            return;
        }

        // 2. NavMeshData 등록
        navMeshInstance = NavMesh.AddNavMeshData(navData, position, rotation);

    }

    public void UnloadNavMesh()
    {
        navMeshInstance.Remove();
    }
}
