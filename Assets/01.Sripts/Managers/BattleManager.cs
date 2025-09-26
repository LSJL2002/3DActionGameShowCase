using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using static GameUI;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject currentMonster; //현재 소환된 몬스터
    public MonsterStatHandler monsterStats; //그몬스터 스텟
    public BattleZone currentZone; // 지금 전투하는 방
    private bool isBattle;

    public static event Action<BattleZone> OnBattleStart;
    public static event Action<BattleZone> OnPlayerDie;
    public static event Action<BattleZone> OnMonsterDie;
    public static event Action<BattleZone> OnBattleClear;


    public async void StartBattle(BattleZone zone)
    {
        if (isBattle) return;
        isBattle = true;
        currentZone = zone;
        var cutScene = currentZone.PlayableDirector;

        // 1. 벽 켜기
        currentZone.SetWallsActive(true);
        OnBattleStart?.Invoke(zone);
        // 2. 연출 시작
        cutScene.Play();

        // 비동기로 몬스터 로드 시작
        var monster = await LoadMonsterPrefab(zone.summonMonsterId);

        // Timeline이 끝날 때까지 대기
        await Task.Delay(TimeSpan.FromSeconds(cutScene.duration));

        // 3.몬스터 소환 완료 대기
        if (monster != null)
            currentMonster = SpawnMonster(monster, zone.transform.position + Vector3.up);

        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentZone == null) return;
            HandleMonsterDie();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentZone != null)
                currentMonster.GetComponent<BaseMonster>().OnTakeDamage(50000);
        }
    }


    public async Task<GameObject> LoadMonsterPrefab(int monsterId)
    {
        string monsterKey = monsterId.ToString();
        var handle = Addressables.LoadAssetAsync<GameObject>(monsterKey);
        GameObject prefab = await handle.Task;

        if (prefab == null)
        {
            Debug.LogError($"몬스터 {monsterId} 프리팹을 찾을 수 없음!");
            return null;
        }
        return prefab;
    }

    public GameObject SpawnMonster(GameObject prefab, Vector3 spawnPos)
    {
        var instance = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);

        currentMonster = instance;
        monsterStats = instance.GetComponent<BaseMonster>().Stats;

        return instance;
    }

    public void HandleMonsterDie()
    {
        if (monsterStats.CurrentHP > 0) return;
        OnMonsterDie?.Invoke(currentZone);
    }


    public void ClearBattle()
    {
        OnBattleClear?.Invoke(currentZone);

        Addressables.ReleaseInstance(currentMonster.gameObject); //갈무리하고나서로 수정
        currentZone = null;
        currentMonster = null;
        monsterStats = null;
        isBattle = false;
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
