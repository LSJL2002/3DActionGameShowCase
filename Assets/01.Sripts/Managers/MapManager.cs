using UnityEngine;
using System.Collections.Generic;
using UnityEngine.WSA;

public class MapManager : MonoBehaviour
{

    private Dictionary<int, BattleZone> zoneDict = new Dictionary<int, BattleZone>();
    private BattleZone currentZone;
    private BattleZone lastBattleZone;

    private void OnEnable()
    {
        //BattleZone.OnBattleZoneEnter += 배틀이 시작됐을 때 호출할 함수
    }

    private void OnDisable()
    {
        //BattleZone.OnBattleZoneEnter -+ 배틀이 끝났을 때 호출할 함수
    }

    private void Start()
    {
        //생성
        //AddressableManager.Instance.MakeGameObject("Map");
        //AddressableManager.Instance.MakeGameObject("BattleZone");

        //딕셔너리에 저장


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
//    foreach (var zone in FindObjectsOfType<BattleZone>(true))
//    {
//        RegisterStage(zone);
//        zone.Deactivate();
//    }

//    // 시작 스테이지만 켜기
//    if (stageDict.TryGetValue(startingStageID, out var startZone))
//    {
//        startZone.Activate();
//    }
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
