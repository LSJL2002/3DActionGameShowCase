using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static GameUI;

public class BattleManager : Singleton<BattleManager>
{
    private GameObject currentMonster;
    public MonsterStatHandler monsterStats;
    private BattleZone activeZone;

    public static event Action<BattleZone> OnBattleStart;
    public static event Action<BattleZone> OnBattleClear;

    public async void StartBattle(BattleZone zone)
    {
        activeZone = zone;

        // 1. 벽 켜기
        activeZone.SetWallsActive(true);

        // 2. 몬스터 소환
        currentMonster = await SpawnMonster(zone.MonsterID, zone.transform.position);

        OnBattleStart?.Invoke(zone);
    }

    public async Task<GameObject> SpawnMonster(int monsterId, Vector3 spawnPos)
    {
        string monsterKey = monsterId.ToString(); // Addressables 키 (등록한 이름이랑 일치해야 함)

        // 프리팹 비동기 로드 & 인스턴스화
        var handle = Addressables.InstantiateAsync(monsterKey, spawnPos, Quaternion.identity);
        GameObject monsterInstance = await handle.Task;

        if (monsterInstance != null)
        {
            currentMonster = monsterInstance;
            Debug.Log($"몬스터 [{monsterId}] 소환 완료!");

            BaseMonster baseMonsterComponent = currentMonster.GetComponent<BaseMonster>();
            monsterStats = baseMonsterComponent.Stats;

            string enemyName = monsterStats.monsterData.monsterName;
            float enemyMaxHP = monsterStats.monsterData.maxHp;

            return monsterInstance; // 호출부에서 받을 수 있음
        }
        else
        {
            Debug.LogError($"몬스터 {monsterId} Addressable 프리팹을 찾을 수 없음! (Key 확인 필요)");
            return null;
        }
    }


    public void ClearBattle()
    {
        OnBattleClear?.Invoke(activeZone);
        if (currentMonster != null)
        {
            //currentMonster.gameObject.SetActive(false);
            Addressables.ReleaseInstance(currentMonster.gameObject);
            currentMonster = null;
            monsterStats = null;
        }
    }




    //    public void LoadMonsterStat(BattleZone zone)
    //{
    //    monsterStats = BattleManager.Instance.monsterStats;
    //    ChangeState(eState.Battle, BattleManager.Instance.monsterStats.monsterData.monsterName, BattleManager.Instance.monsterStats.monsterData.maxHp);
    //}

    //BattleManager.OnBattleStart += LoadMonsterStat; //이벤트에 등록하면 해당배틀존에 들어가면 발동함
}
