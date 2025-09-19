using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static GameUI;

public class BattleManager : Singleton<BattleManager>
{
    private GameObject currentMonster; //현재 소환된 몬스터
    public MonsterStatHandler monsterStats; //그몬스터 스텟
    private BattleZone currentZone; // 지금 전투하는 방

    public static event Action<BattleZone> OnBattleStart;
    public static event Action<BattleZone> OnBattleClear;

    public async void StartBattle(BattleZone zone)
    {
        currentZone = zone;


        // 1. 벽 켜기
        currentZone.SetWallsActive(true);

        // 2. 몬스터 소환
        currentMonster = await SpawnMonster(zone.summonMonsterId, zone.transform.position);

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
        OnBattleClear?.Invoke(currentZone);
        if (currentMonster != null)
        {
            //currentMonster.gameObject.SetActive(false);
            Addressables.ReleaseInstance(currentMonster.gameObject); //갈무리하고나서로 수정
            currentMonster = null;
            monsterStats = null;
        }
    }

    public string GetItemInfo(int index)
    {
        if (index >= 0 && index < currentZone.getableItemTable.Count)
        {
            // 2. 유효한 경우, 해당 인덱스의 값을 반환합니다.
            return currentZone.getableItemTable[index].ToString();
        }
        else
        {
            // 3. 유효하지 않은 경우, 오류 로그를 출력하고 null을 반환합니다.
            Debug.LogError($"Invalid index: {index}. The list has only {currentZone.getableItemTable.Count} items.");
            return null;
        }
    }



    //    public void LoadMonsterStat(BattleZone zone)
    //{
    //    monsterStats = BattleManager.Instance.monsterStats;
    //    ChangeState(eState.Battle, BattleManager.Instance.monsterStats.monsterData.monsterName, BattleManager.Instance.monsterStats.monsterData.maxHp);
    //}

    //BattleManager.OnBattleStart += LoadMonsterStat; //이벤트에 등록하면 해당배틀존에 들어가면 발동함
}
