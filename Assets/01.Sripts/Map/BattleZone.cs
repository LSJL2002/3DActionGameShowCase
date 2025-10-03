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
    [SerializeField] private BattleZoneSO ZoneData;


    [Header("타임라인키")]
     public string startBattleTimelineKey;
     public string endBattleTimelineKey;


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

    private async void OnTriggerEnter(Collider other)
    {
        UIManager.Instance.currentDecisionState = DecisionState.EnterToZone;

        if (other.CompareTag("Player"))
        {
            DecisionButtonUI decisionUI = await UIManager.Instance.Show<DecisionButtonUI>();

            CharacterController playerCC = other.GetComponent<CharacterController>();

            // 이벤트 구독을 위한 델리게이트 변수 생성
            Action<bool> onDecisionMadeCallback = null;
            onDecisionMadeCallback = (isConfirmed) =>
            {
                // 확인UI에서 허가를 받았다면,
                if (isConfirmed)
                {
                    BattleManager.Instance.StartBattle(this);
                }
                else
                {
                    // CharacterController가 있다면 잠시 비활성화
                    playerCC.enabled = false;

                    // 플레이어를 뒤로 물러나게 함
                    Vector3 newPosition = other.transform.position + new Vector3(0, 0, -5f);
                    other.transform.position = newPosition;

                    // 비활성화 했던 CharacterController를 다시 활성화
                    playerCC.enabled = true;
                }
                decisionUI.OnDecisionMade -= onDecisionMadeCallback; // 이벤트 구독 해제
            };
            decisionUI.OnDecisionMade += onDecisionMadeCallback; // 이벤트 구독
        }
    }

    public void SetWallsActive(bool active) => walls?.SetActive(active);
}