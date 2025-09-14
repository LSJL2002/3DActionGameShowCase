using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private GameObject currentMonster;
    public MonsterStatHandler monsterStats;
    private BattleZone activeZone;

    public static event Action<BattleZone> OnBattleStart;
    public static event Action<BattleZone> OnBattleClear;

    public void StartBattle(BattleZone zone)
    {
        activeZone = zone;

        // 1. 벽 켜기
        activeZone.SetWallsActive(true);

        // 2. 몬스터 소환
        SpawnMonster(zone.MonsterID, zone.transform.position); //위치 설정가능

        OnBattleStart?.Invoke(zone);
    }
    

    public void SpawnMonster(int monsterId, Vector3 spawnPos)
    {
        string path = $"Monster/{monsterId}";
        GameObject prefab = Resources.Load<GameObject>(path); //Addressable로 변경

        if (prefab != null)
        {
            currentMonster = Instantiate(prefab, spawnPos, Quaternion.identity);
            Debug.Log($"몬스터 {monsterId} 소환 완료!");

            BaseMonster baseMonsterComponent = currentMonster.GetComponent<BaseMonster>();
            monsterStats = baseMonsterComponent.Stats;

            string enemyName = monsterStats.monsterData.monsterName;
            float enemyMaxHP = monsterStats.monsterData.maxHp;
        }
        else
        {
            Debug.LogError($"몬스터 {monsterId} 프리팹을 Resources/Monsters/ 폴더에서 찾을 수 없음!");
        }
    }

    public void ClearBattle()
    {
        OnBattleClear?.Invoke(activeZone);
        if (currentMonster != null)
        {
            currentMonster.gameObject.SetActive(false);
            //Addressables.ReleaseInstance(currentMonster.gameObject);
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
