using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public GameObject[] MonsterPrefab;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        BattleZone.OnBattle += SpawnMonster;
        BattleZone.OnBattleClear += NextStage;
    }

    private void OnDisable()
    {
        BattleZone.OnBattle -= SpawnMonster;
        BattleZone.OnBattleClear -= NextStage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnMonster(BattleZone battleZone)
    {
        int random = Random.Range(0, MonsterPrefab.Length);
        GameObject monster = Instantiate(MonsterPrefab[random], spawnPoint.position, Quaternion.identity);
        Debug.Log($"{random}번째인 {MonsterPrefab[random].name}이 {spawnPoint.position.x},{spawnPoint.position.y},{spawnPoint.position.z}에 소환됨");
    }

    private void NextStage(BattleZone battleZone)
    {
        Debug.Log("다음스테이지가 열렸습니다");
    }
}
