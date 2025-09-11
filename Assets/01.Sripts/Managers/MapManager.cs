using UnityEngine;
using System.Collections.Generic;
using UnityEngine.WSA;

public class MapManager : MonoBehaviour
{

    [Header("Stage Flow")]
    public int startingStageID;

    private Dictionary<int, BattleZone> stageDict = new();
    private BattleZone currentZone;

    void OnEnable()
    {
        BattleZone.OnBattle += HandleZoneEnter;
        BattleZone.OnBattleClear += HandleZoneClear;
    }

    void OnDisable()
    {
        BattleZone.OnBattle -= HandleZoneEnter;
        BattleZone.OnBattleClear -= HandleZoneClear;
    }

    void Start()
    {
        // 씬에 있는 모든 BattleZone 등록
        foreach (var zone in FindObjectsOfType<BattleZone>(true))
        {
            RegisterStage(zone);
        }

        // 시작 스테이지만 켜기
        if (stageDict.TryGetValue(startingStageID, out var startZone))
        {
            startZone.Activate();
        }
    }

    public void RegisterStage(BattleZone zone)
    {
        if (!stageDict.ContainsKey(zone.stageID))
            stageDict.Add(zone.stageID, zone);
    }

    private void HandleZoneEnter(BattleZone zone)
    {
        currentZone = zone;
        zone.StartBattle();
    }

    private void HandleZoneClear(BattleZone zone)
    {
        if (!stageDict.ContainsKey(zone.stageID)) return;

        // 다음 스테이지 열기
        foreach (var id in zone.nextStages)
        {
            var next = GetStage(id);
            if (next != null) next.Activate();
        }

        if (zone.isEndingStage)
        {
            Debug.Log("모든 스테이지 클리어! 엔딩씬으로 이동!");
            // SceneManager.LoadScene("EndingScene");
        }

        currentZone = null;
    }

    public BattleZone GetStage(int id)
    {
        stageDict.TryGetValue(id, out var zone);
        return zone;
    }

    public void Activate()
    {

    }
}
