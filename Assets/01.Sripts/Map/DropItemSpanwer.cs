using UnityEngine;

public class DropItemSpawner : MonoBehaviour
{
    [field : SerializeField]public GameObject dropItem { get; set; }// 인스펙터에 드랍 아이템 넣어두기

    private void Start()
    {
        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnMonsterDie, SpawnItem); // 구독해제
        EventsManager.Instance.StopListening<BattleZone>(GameEventT.OnBattleClear, HideItem); // 구독해제

        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnMonsterDie, SpawnItem); // 구독
        EventsManager.Instance.StartListening<BattleZone>(GameEventT.OnBattleClear, HideItem); // 구독
    }

    private void SpawnItem(BattleZone zone)
    {
        // 배틀존 클리어 → 아이템 위치 이동 후 켜주기
        Vector3 dropPos = BattleManager.Instance.currentMonster.transform.position; // 몬스터위치
        dropPos.y = 0;
        dropItem.transform.position = dropPos;
        //dropItem.transform.position = PlayerManager.Instance.stateMachine.Player.transform.position; //플레이어 위치
        dropItem.SetActive(true);
    }

    private void HideItem(BattleZone zone)
    {
        dropItem.SetActive(false);
    }
}