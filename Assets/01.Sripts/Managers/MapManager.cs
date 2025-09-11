using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public GameObject[] MonsterPrefab;
    public Transform[] spawnPoint;

    public int index = 0;

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
        GameObject monster = Instantiate(MonsterPrefab[random], battleZone.spawnPoint.position, Quaternion.identity);
        //적에 대한 컷신
    }

    private void NextStage(BattleZone battleZone)
    {
        Debug.Log("다음스테이지 오픈");
    }
}
