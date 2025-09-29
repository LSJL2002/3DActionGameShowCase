using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using static GameUI;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject currentMonster; //현재 소환된 몬스터
    public MonsterStatHandler monsterStats; //그몬스터 스텟
    public BattleZone currentZone; // 지금 전투하는 방
    private bool isBattle;

    public static event Action<BattleZone> OnBattleStart;
    public static event Action<BattleZone> OnPlayerDie;
    public static event Action<BattleZone> OnMonsterDie;
    public static event Action<BattleZone> OnBattleClear;

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
        await TimeLineManager.Instance.OnTimeLine<PlayableDirector>("TimeLine_SMachineBattleStart");

        if (isBattle) return;
        isBattle = true;
        currentZone = zone;
        var cutScene = currentZone.PlayableDirector;

        // 1. 벽 켜기
        currentZone.SetWallsActive(true);
        StartWarning();

        // 2. 연출 시작
        cutScene.Play();

        // 플레이 상태 체크 (연출 시작 직후)
        if (!Application.isPlaying || this == null) return;

        // Timeline 끝날 때까지 대기
        await Task.Delay(TimeSpan.FromSeconds(cutScene.duration));

        // 플레이 상태 체크 (대기 후)
        if (!Application.isPlaying || this == null) return;

        // 3. 몬스터 소환
        currentMonster = await SpawnMonster(zone.summonMonsterId, zone.spawnPoint.position);

        // 플레이 상태 체크 (몬스터 로드 끝난 뒤)
        if (!Application.isPlaying || this == null || currentMonster == null) return;

        currentMonster.transform.LookAt(PlayerManager.Instance.transform.position);

        OnBattleStart?.Invoke(zone);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentZone == null) return;
            HandleMonsterDie();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentZone != null)
                currentMonster.GetComponent<BaseMonster>().OnTakeDamage(50000);
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

    public void HandleMonsterDie()
    {
        if (monsterStats.CurrentHP > 0) return;
        OnMonsterDie?.Invoke(currentZone);
        StopWarning();
    }


    public void ClearBattle()
    {
        OnBattleClear?.Invoke(currentZone);

        Addressables.ReleaseInstance(currentMonster.gameObject); //갈무리하고나서로 수정
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
            // 3. 유효하지 않은 경우, 오류 로그를 출력하고 null을 반환합니다.
            Debug.LogError($"Invalid index: {index}. The list has only {currentZone.getableItemTable.Count} items.");
            return null;
        }
    }


    //    public void LoadMonsterStat(BattleZone zone)
    //{
    //    monsterStats = BattleManager.Instance.monsterStats;
    //    ChangeState(eState.Battle, BattleManager.Instance.monsterStats.monsterData.monsterName, BattleManager.Instance.monsterStats.monsterData.maxHp);
    //}

    //BattleManager.OnBattleStart += LoadMonsterStat; //이벤트에 등록하면 해당배틀존에 들어가면 발동함
}
