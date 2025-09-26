using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleZone : MonoBehaviour
{
    [Header("스테이지 정보")]
    public int id;
    public string stageName;
    public int summonMonsterId;
    public List<int> moveAbleStage;
    public List<int> getableItemTable;
    public float extraHP;
    public float extraMP;
    public int extraATK;
    public int extraDEF;
    public float extraSpeed;
    public float extraAtkSpeed;

    [Header("못나가게막는벽")]
    [SerializeField] private GameObject walls;

    [SerializeField]
    private BattleZoneSO ZoneData;

    //public PlayableDirector PlayableDirector;

    private void Awake()
    {
        if (ZoneData != null)
        {
            id = ZoneData.id;
            stageName = ZoneData.stageName;
            summonMonsterId = ZoneData.summonMonsterId;
            moveAbleStage = ZoneData.moveAbleStage;
            getableItemTable = ZoneData.getableItemTable;
            extraHP = ZoneData.extraHP;
            extraMP = ZoneData.extraMP;
            extraATK = ZoneData.extraATK;
            extraDEF = ZoneData.extraDEF;
            extraSpeed = ZoneData.extraSpeed;
            extraAtkSpeed = ZoneData.extraAtkSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BattleManager.Instance.StartBattle(this);

            //PlayableDirector.Play();
        }
    }

    public void SetWallsActive(bool active) => walls?.SetActive(active);

}


//[SerializeField]
//private bool isClear;

//[Header("Zone Objects")]
//public GameObject[] walls;
//public GameObject _monster;


//[Header("Zone Data")]
//public Transform spawnPoint;
//public int stageID;
//public int[] nextStages;
//public bool isEndingStage;

//private void Start()
//{
//    foreach (GameObject wall in walls)
//    {
//        wall.SetActive(false);
//    }

//    if (_monster != null)
//    {
//        _monster.SetActive(false);
//    }
//}

//public static event Action<BattleZone> OnBattle;
//public static event Action<BattleZone> OnBattleClear;


//private void Update()
//{
//    if (isClear) // 이벤트 호출형식으로 수정
//    {
//        //foreach (GameObject wall in walls)
//        //{
//        //    wall.SetActive(false);
//        //}
//        //_monster.SetActive(false);

//        //if(nextStages != null)
//        //{
//        //    foreach (int stage in nextStages)
//        //    {
//        //        gameObject.SetActive(true);
//        //    }
//        //}
//        Deactivate();


//    }

//    if (Input.GetMouseButtonDown(1)) //몬스터가 사망했을때로 수정
//    {
//        isClear = !isClear;
//        OnBattleClear?.Invoke(this);
//    }
//}

//private void OnTriggerEnter(Collider other)
//{
//    if (other.CompareTag("Player") && !isClear)
//    {
//        OnBattle?.Invoke(this);
//    }
//}

//public void StartBattle()
//{
//    Debug.Log("전투가 시작됩니다.");

//    foreach (GameObject wall in walls)
//    {
//        wall.SetActive(true);
//    }
//    SpawnMonster();

//}

//private void SpawnMonster()
//{

//    GameObject monster = Instantiate(_monster, spawnPoint.position, Quaternion.identity);
//    _monster = monster;
//    monster.SetActive(true);
//    //적에 대한 컷신
//}


//public void Activate()
//{
//    gameObject.SetActive(true);
//}
//public void Deactivate()
//{
//    gameObject.SetActive(false);
//}