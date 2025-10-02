using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillManagers : MonoBehaviour
{
    [Serializable]
    public class SkillPrefab
    {
        public string skillName;
        public GameObject prefab;
        public int poolSize = 1; // 기본 풀 크기
    }

    [Header("스킬 프리팹")]
    public SkillPrefab[] skillPrefabs;

    // 스킬 이름 → 풀(Queue)
    private Dictionary<string, Queue<GameObject>> skillPools = new();


    private void Awake()
    {
        // 각 스킬 풀 초기화
        foreach (var skill in skillPrefabs)
        {
            if (skill.prefab == null) continue;

            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < skill.poolSize; i++)
            {
                var obj = Instantiate(skill.prefab, transform); // 월드 위치와 상관없이 씬 상에 둬도 됨
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            skillPools[skill.skillName] = pool;
        }
    }


    /// <summary>
    /// 스킬 발동
    /// </summary>
    public GameObject SpawnSkill(string skillName, Vector3 position, Quaternion rotation = default)
    {
        if (!skillPools.TryGetValue(skillName, out var pool) || pool.Count == 0) return null;

        // 풀에서 오브젝트 가져오기
        var obj = pool.Dequeue();
        obj.transform.position = position;
        // 디폴트 회전 처리
        if (rotation == default) rotation = Quaternion.identity;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // 파티클 재생
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        ps?.Play(true);

        // Hitbox 활성화
        var hitbox = obj.GetComponentInChildren<Hitbox>();
        hitbox?.OnEnable();

        // 사운드
        AudioManager.Instance?.PlaySFX(skillName);

        // 일정 시간 후 반환 처리
        StartCoroutine(ReturnAfterParticle(obj, ps, hitbox, pool));

        return obj;
    }

    private IEnumerator ReturnAfterParticle(GameObject obj, ParticleSystem ps, Hitbox hitbox, Queue<GameObject> pool)
    {
        if (ps != null)
        {
            while (ps.IsAlive(true))
                yield return null;

            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // Hitbox 비활성화
        hitbox?.OnDisable();

        // 오브젝트 풀로 반환
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}