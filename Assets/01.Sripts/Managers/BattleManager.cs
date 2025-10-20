using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject currentMonster; //현재 소환된 몬스터
    public MonsterStatHandler monsterStats; //그몬스터 스텟
    public BattleZone currentZone; // 지금 전투하는 방
    private bool isBattle;

    //[SerializeField] private BaseEventSO<BattleZone> OnBattleStart;
    //[SerializeField] private BaseEventSO<BattleZone> OnPlayerDie;
    //[SerializeField] private BaseEventSO<BattleZone> OnMonsterDie;
    //[SerializeField] private BaseEventSO<BattleZone> OnBattleClear;

    [SerializeField] private Material warningMaterial;
    private Tween emissionTween;
    private static readonly Color baseEmission = Color.black;
    private static readonly Color warningEmission = new Color(100f / 255f, 100f / 255f, 0f / 255f);
    private static readonly Color safeEmission = new Color(18f / 255f, 1f, 0f / 255f);

    private void StartWarning()
    {
        if (warningMaterial == null) return;
        warningMaterial.EnableKeyword("_EMISSION");

        // 시작 전에 반드시 기본값으로 세팅
        warningMaterial.SetColor("_EmissionColor", baseEmission);

        emissionTween?.Kill();
        emissionTween = DOTween.To(
            () => warningMaterial.GetColor("_EmissionColor"),
            x => warningMaterial.SetColor("_EmissionColor", x),
            warningEmission,
            0.8f
        ).SetLoops(-1, LoopType.Yoyo)
         .SetUpdate(true)
         .SetTarget(warningMaterial);
    }


    private void StopWarning()
    {
        emissionTween?.Kill();
        emissionTween = null;
        warningMaterial.SetColor("_EmissionColor", safeEmission);
    }


    //public async void StartBattle(BattleZone zone)
    //{
    //    var cutScene =  await TimeLineManager.Instance.OnTimeLine<PlayableDirector>("TimeLine_SMachineBattleStart");

    //    if (isBattle) return;
    //    isBattle = true;
    //    currentZone = zone;

    //    // 1. 벽 켜기
    //    currentZone.SetWallsActive(true);
    //    StartWarning();


    //    // 플레이 상태 체크 (연출 시작 직후)
    //    if (!Application.isPlaying || this == null) return;

    //    // Timeline 끝날 때까지 대기
    //    await Task.Delay(TimeSpan.FromSeconds(cutScene.duration));

    //    // 플레이 상태 체크 (대기 후)
    //    if (!Application.isPlaying || this == null) return;

    //    // 3. 몬스터 소환
    //    currentMonster = await SpawnMonster(zone.summonMonsterId, zone.transform.position);

    //    // 플레이 상태 체크 (몬스터 로드 끝난 뒤)
    //    if (!Application.isPlaying || this == null || currentMonster == null) return;

    //    currentMonster.transform.LookAt(PlayerManager.Instance.transform.position);

    //    OnBattleStart?.Invoke(zone);
    //}

    public async void StartBattle(BattleZone zone)
    {
        if (isBattle) return;
        isBattle = true;
        currentZone = zone;
        currentZone.triggerCollider.enabled = false;
        // 1. 벽 켜기 + 경고 이펙트 시작
        currentZone.SetWallsActive(true);
        //StartWarning();

        // 2. 컷씬 실행 & 종료 대기 (스킵 포함)
        if (!string.IsNullOrEmpty(zone.startBattleTimelineKey))
        {
            Debug.Log($"Battle Start → Timeline 실행: {currentZone.startBattleTimelineKey}");
            await TimeLineManager.Instance.PlayAndWait(currentZone.startBattleTimelineKey);
        }

        // 게임 종료 중이면 리턴
        if (!Application.isPlaying || this == null) return;

        // 3. 몬스터 소환
        currentMonster = await SpawnMonster(zone.summonMonsterId, zone.transform.position);

        if (currentMonster != null)
        {
            currentMonster.transform.LookAt(PlayerManager.Instance.transform.position);
            Debug.Log("Battle Start → 몬스터 소환 완료");
        }
        else
        {
            Debug.LogError("Battle Start → 몬스터 소환 실패");
        }

        // 4. 전투 시작 이벤트 브로드캐스트
        EventsManager.Instance.TriggerEvent<BattleZone>(GameEventT.OnBattleStart, zone);
    }




    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentZone == null) return;
            HandleMonsterDie();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentZone != null)
                currentMonster.GetComponent<BaseMonster>().OnTakeDamage(10000000);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if(currentZone != null)
            {
                PlayerManager.Instance.Attr.Resource.TakeDamage(5000);
            }
        }
    }

    public async Task<GameObject> SpawnMonster(int monsterId, Vector3 spawnPos)
    {
        string monsterKey = monsterId.ToString();

        // 로드 + 인스턴스화를 한 번에
        var handle = Addressables.InstantiateAsync(monsterKey, spawnPos, Quaternion.identity);

        // 플레이 꺼지면 더 진행하지 않도록 체크
        if (!Application.isPlaying)
            return null;

        GameObject instance = await handle.Task;

        if (instance != null)
        {
            currentMonster = instance;
            monsterStats = instance.GetComponent<BaseMonster>().Stats;
        }

        return instance;
    }

    public async UniTask HandleMonsterDie()
    {
        if (monsterStats == null || currentZone == null || monsterStats.CurrentHP > 0)
            return;

        if (currentZone.id == MapManager.Instance.bossZoneId)
        {
            await TimeLineManager.Instance.OnTimeLine<PlayableDirector>(currentZone.endBattleTimelineKey);
            MapManager.Instance.HandleLastStageClear();
            return;
        }

        await TimeLineManager.Instance.OnTimeLine<PlayableDirector>(currentZone.endBattleTimelineKey);
        EventsManager.Instance.TriggerEvent<BattleZone>(GameEventT.OnMonsterDie, currentZone);

        //StopWarning();
    }


    public void ClearBattle()
    {
        EventsManager.Instance.TriggerEvent<BattleZone>(GameEventT.OnBattleClear, currentZone);
        SaveManager.Instance.SetStageData(currentZone.id);
        SaveManager.Instance.SaveData();

        Addressables.ReleaseInstance(currentMonster.gameObject); 
        currentZone = null;
        currentMonster = null;
        monsterStats = null;
        isBattle = false;
        
    }

    public void ResetBattleState()
    {
        currentZone = null;
        currentMonster = null;
        monsterStats = null;
        isBattle = false;

        Debug.Log("BattleManager → 배틀 상태 리셋 완료");
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

}





//몬스터 체력별 대사
//OnTakeDamage
// UIManager.Instance.Show<TutorialUI>();
//UIManager.Instance.Get<TutorialUI>().TryPlayBossThresholdDialogue(SceneType.Boss_1);

//public void TryPlayBossThresholdDialogue(SceneType type)
//{
//    float hpPercent = BattleManager.Instance.monsterStats.CurrentHP / BattleManager.Instance.monsterStats.maxHp;
