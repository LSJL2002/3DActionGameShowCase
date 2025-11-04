using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject currentMonster; //현재 소환된 몬스터
    public MonsterStatHandler monsterStats; //그몬스터 스텟
    public BattleZone currentZone; // 지금 전투하는 방
    private bool isBattle;
    private float battleStartTime;

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

    public async void StartBattle(BattleZone zone)
    {
        if (isBattle) return;
        
        isBattle = true;
        AudioManager.Instance.StopBGM();
        battleStartTime = Time.time;

        currentZone = zone;
        CheckInBattle();                           // 퍼널10번
        currentZone.triggerCollider.enabled = false;
        // 1. 벽 켜기 + 경고 이펙트 시작
        currentZone.SetWallsActive(true);
        StartWarning();

        // 2. 컷씬 실행 & 종료 대기 (스킵 포함)
        if (!string.IsNullOrEmpty(zone.TimeLineOP))
        {
            CheckPlayOPCutScene();
          
            await TimeLineManager.Instance.PlayAndWait(currentZone.TimeLineOP);


            // 게임 종료 중이면 리턴
            if (!Application.isPlaying || this == null) return;

            // 3. 몬스터 소환
            currentMonster = await SpawnMonster(zone.summonMonsterId, zone.transform.position);

            if (currentMonster != null)
            {
                currentMonster.transform.LookAt(PlayerManager.Instance.transform.position);
               
            }
           

            // 4. 전투 시작 이벤트 브로드캐스트
            EventsManager.Instance.TriggerEvent<BattleZone>(GameEventT.OnBattleStart, zone);

            var evt = new CustomEvent("battle_start")
        {
            { "zoneName", currentZone.stageName },
            { "timestamp", System.DateTime.UtcNow.ToString("o") }
        };
            AnalyticsService.Instance.RecordEvent(evt);
            AudioManager.Instance.PlayBGM("BattleBGM");

        }
    }




    protected override void Update()
    {
        if (Time.timeScale != 1f) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentZone != null)
                currentMonster.GetComponent<BaseMonster>().OnTakeDamage(10000000);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (currentZone != null)
                currentMonster.GetComponent<BaseMonster>().OnTakeDamage(6000);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (currentZone != null)
                currentMonster.GetComponent<BaseMonster>().OnTakeDamage(12000);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (currentZone != null)
            {
                PlayerManager.Instance.Attr.Resource.TakeDamage(5000);
            }
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            MapManager.Instance.HandleLastStageClear();
            MapManager.Instance.gameClearDoor.transform.position = PlayerManager.Instance.ActiveCharacter.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            var controller = PlayerManager.Instance.ActiveCharacter.GetComponent<CharacterController>();
            MapManager.Instance.startingZoneId = MapManager.Instance.bossZoneId;
            MapManager.Instance.ReturnToStartZone();
            if (PlayerManager.Instance != null)
            {
                controller.enabled = false;
                PlayerManager.Instance.ActiveCharacter.transform.position = MapManager.Instance.currentZone.transform.position;
                controller.enabled = true;
            }
            MapManager.Instance.startingZoneId = 60000000;


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
        AudioManager.Instance.StopBGM();
        if (monsterStats == null || currentZone == null || monsterStats.CurrentHP > 0)
            return;
        currentMonster.GetComponent<BoxCollider>().enabled = false;

        float elapsed = Time.time - battleStartTime; // 전투 시작 후 경과 시간
        currentMonster.transform.position = currentZone.transform.position;

        // 아날리틱스 전송
        var evt = new CustomEvent("monster_death")
    {
        { "zoneName", currentZone.stageName },
        { "monsterName", currentMonster.name },
        { "elapsedTime", elapsed },                           // 몬스터를 죽이는데 걸린시간
        { "eventTime", System.DateTime.UtcNow.ToString("o") } // 실제시간 - 언제 이벤트가 발생했는지
    };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
        Debug.Log($"[Analytics] monster_death → {currentMonster.name}, time={elapsed:F2}s, zone={currentZone.stageName}");
        CheckPlayEDCutScene();
        await TimeLineManager.Instance.PlayAndWait(currentZone.TimeLineED);

        EventsManager.Instance.TriggerEvent<BattleZone>(GameEventT.OnMonsterDie, currentZone);
        if (currentZone.id == MapManager.Instance.bossZoneId)
        {
            MapManager.Instance.HandleLastStageClear();
        }

        await UniTask.NextFrame();
        StopWarning();
        AudioManager.Instance.PlayBGM("afterBattleBGM");

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
          
          
            return null;
        }
    }


    private void CheckInBattle()
    {
        switch (currentZone.id)
        {
            case 60000000:
                AnalyticsManager.SendFunnelStep("10");
                break;
            case 60000001:
                AnalyticsManager.SendFunnelStep("20");
                break;
            case 60000002:
                AnalyticsManager.SendFunnelStep("20");
                break;
            case 60000003:
                AnalyticsManager.SendFunnelStep("20");
                break;
            case 60000004:
                AnalyticsManager.SendFunnelStep("30");
                break;
            case 60000005:
                AnalyticsManager.SendFunnelStep("30");
                break;
            case 60000006:
                AnalyticsManager.SendFunnelStep("30");
                break;
            case 60000007:
                AnalyticsManager.SendFunnelStep("40");
                break;
            case 60000008:
                AnalyticsManager.SendFunnelStep("40");
                break;
            case 60000009:
                AnalyticsManager.SendFunnelStep("40");
                break;
            case 60000010:
                AnalyticsManager.SendFunnelStep("50");
                break;
        }
    }

    private void CheckPlayOPCutScene()
    {
        switch (currentZone.id)
        {
            case 60000000:
                AnalyticsManager.SendFunnelStep("11");
                break;
            case 60000001:
                AnalyticsManager.SendFunnelStep("21");
                break;
            case 60000002:
                AnalyticsManager.SendFunnelStep("21");
                break;
            case 60000003:
                AnalyticsManager.SendFunnelStep("21");
                break;
            case 60000004:
                AnalyticsManager.SendFunnelStep("31");
                break;
            case 60000005:
                AnalyticsManager.SendFunnelStep("31");
                break;
            case 60000006:
                AnalyticsManager.SendFunnelStep("31");
                break;
            case 60000007:
                AnalyticsManager.SendFunnelStep("41");
                break;
            case 60000008:
                AnalyticsManager.SendFunnelStep("41");
                break;
            case 60000009:
                AnalyticsManager.SendFunnelStep("41");
                break;
            case 60000010:
                AnalyticsManager.SendFunnelStep("51");
                break;
        }
    }

    private void CheckPlayEDCutScene()
    {
        switch (currentZone.id)
        {
            case 60000000:
                AnalyticsManager.SendFunnelStep("17");
                break;
            case 60000001:
                AnalyticsManager.SendFunnelStep("27");
                break;
            case 60000002:
                AnalyticsManager.SendFunnelStep("27");
                break;
            case 60000003:
                AnalyticsManager.SendFunnelStep("27");
                break;
            case 60000004:
                AnalyticsManager.SendFunnelStep("37");
                break;
            case 60000005:
                AnalyticsManager.SendFunnelStep("37");
                break;
            case 60000006:
                AnalyticsManager.SendFunnelStep("37");
                break;
            case 60000007:
                AnalyticsManager.SendFunnelStep("47");
                break;
            case 60000008:
                AnalyticsManager.SendFunnelStep("47");
                break;
            case 60000009:
                AnalyticsManager.SendFunnelStep("47");
                break;
            case 60000010:
                AnalyticsManager.SendFunnelStep("57");
                break;
        }
    }
}

