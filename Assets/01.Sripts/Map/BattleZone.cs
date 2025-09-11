using System;
using UnityEngine;

public class BattleZone : MonoBehaviour
{

    [SerializeField]
    private bool isClear;

    [Header("Zone Objects")]
    public GameObject[] walls;
    public GameObject _monster;


    [Header("Zone Data")]
    public Transform spawnPoint;
    public int stageID;
    public int[] nextStages;
    public bool isEndingStage;

    private void Start()
    {
        gameObject.SetActive(false);
        foreach (GameObject wall in walls)
        {
            wall.SetActive(false);
        }

        if(_monster != null)
        {
            _monster.SetActive(false);
        }
    }

    public static event Action<BattleZone> OnBattle;
    public static event Action<BattleZone> OnBattleClear;


    private void Update()
    {
        if (isClear) // 이벤트 호출형식으로 수정
        {
            //foreach (GameObject wall in walls)
            //{
            //    wall.SetActive(false);
            //}
            //_monster.SetActive(false);

            //if(nextStages != null)
            //{
            //    foreach (int stage in nextStages)
            //    {
            //        gameObject.SetActive(true);
            //    }
            //}
            Deactivate();


        }

        if (Input.GetMouseButtonDown(0)) //몬스터가 사망했을때로 수정
        {
            isClear = !isClear;
            OnBattleClear?.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!isClear)
        {
          StartBattle();           
        } 
    }

    public void StartBattle()
    {
        Debug.Log("전투가 시작됩니다.");

        foreach (GameObject wall in walls)
        {
            wall.SetActive(true);
            Debug.Log($"{wall.name}이 켜졌습니다.");
        }
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        GameObject monster = Instantiate(_monster, spawnPoint.position, Quaternion.identity);
        monster.SetActive(true);
        //적에 대한 컷신
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }


    //public void Clear()
    //{
    //    isClear = true;
    //    foreach (var next in nextStages)
    //    {
    //        if (next != null) next.Activate();
    //    }
    //}
}
