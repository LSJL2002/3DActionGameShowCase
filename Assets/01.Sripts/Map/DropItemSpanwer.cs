using UnityEngine;

public class DropItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dropItem; // 인스펙터에 드랍 아이템 넣어두기

    private void OnEnable()
    {
        BattleManager.OnMonsterDie += HandleZoneClear;
    }

    private void OnDisable()
    {
        BattleManager.OnMonsterDie -= HandleZoneClear;
    }

    private void HandleZoneClear(BattleZone zone)
    {
        // 배틀존 클리어 → 아이템 위치 이동 후 켜주기
        Vector3 dropPos = BattleManager.Instance.currentMonster.transform.position; // 몬스터위치
        dropItem.transform.position = dropPos;
        dropItem.SetActive(true);
    }
}