using UnityEngine;
using System.Collections.Generic;
using UnityEngine.WSA;

public class MapManager : Singleton<MapManager>
{

    private Dictionary<int, BattleZone> zoneDict = new Dictionary<int, BattleZone>();
    public BattleZone currentZone = null;

    [SerializeField]private int startingZoneId;
    [SerializeField] private int BossZoneId;

    private void OnEnable()
    {
        BattleZone.OnBattleZoneEnter += HandleZoneEnter;//배틀이 시작됐을 때 호출할 함수
        BattleZone.OnBattleZoneClear += HandleZoneClear;
    }

    private void OnDisable()
    {
        BattleZone.OnBattleZoneEnter -= HandleZoneEnter; //배틀이 끝났을 때 호출할 함수
        BattleZone.OnBattleZoneClear -= HandleZoneClear;
    }

    private void Start()
    {
        //생성
        //AddressableManager.Instance.MakeGameObject("Map");
        //AddressableManager.Instance.MakeGameObject("BattleZone");

        //배틀존 딕셔너리에 아이디를 키값으로 등록
        var zones = FindObjectsOfType<BattleZone>(); //배틀존을 싹다 찾음
        foreach (var zone in zones)
        {
            RegisterStage(zone);    //딕셔너리에 추가
            zone.SetWallsActive(false); //벽먼저꺼줌
            zone.gameObject.SetActive(false);  //전부 꺼줌
        }

        if (zoneDict.TryGetValue(startingZoneId, out var startZone)) //startingZoneId를 가진 배틀존을 찾아서
        {
         startZone.gameObject.SetActive(true);   //시작스테이지만 켜줌
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentZone == null) return;
            HandleZoneClear(currentZone);
        }
    }


    public void RegisterStage(BattleZone zone)
    {
        if(!zoneDict.ContainsKey(zone.zoneID)) //아이디가없으면 추가
            zoneDict.Add(zone.zoneID, zone);
    }

    private void HandleZoneEnter(BattleZone zone) //입장시
    {
        currentZone = zone;
        zone.SetWallsActive(true); //벽켜줌

        foreach (var otherZone in zoneDict)  //나머지 스테이지 꺼줌
        {
            if (otherZone.Value != zone)
            {
                otherZone.Value.gameObject.SetActive(false);
            }
        }
    }

    private void HandleZoneClear(BattleZone zone) // 클리어시
    {
        if (zone.nextZoneID == null || zone.nextZoneID.Length == 0)
        {
            Debug.Log("마지막 스테이지 클리어!");
            
            // SceneManager.LoadScene("Stage2");
            return;
        }

        foreach (var nextId in zone.nextZoneID)
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
        zoneDict.Remove(zone.zoneID);
        //Addressables.ReleaseInstance(zone.gameObject);
        currentZone = null;

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
