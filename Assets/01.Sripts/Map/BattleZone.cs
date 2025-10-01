using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

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

    [Header("타임라인키")]
    public string timelineKey;


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
        }
    }

    public void SetWallsActive(bool active) => walls?.SetActive(active);

}





