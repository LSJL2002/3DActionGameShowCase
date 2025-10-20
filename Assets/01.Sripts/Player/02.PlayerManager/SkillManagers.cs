using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// 근접 + 원거리 공용 스킬 호출
    /// </summary>
    public GameObject SpawnSkill(string skillName, Vector3 position, Quaternion rotation = default,
        Action<IDamageable, Vector3> onHit = null)
    {
        if (!skillPools.TryGetValue(skillName, out var pool))
            return null;

        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(
                    skillPrefabs.FirstOrDefault(s => s.skillName == skillName)?.prefab, transform);

        obj.transform.position = position;
        obj.transform.rotation = rotation == default ? Quaternion.identity : rotation;
        obj.SetActive(true);

        // 파티클 재생
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        ps.Play(true);  // 새로 재생
        AudioManager.Instance?.PlaySFX(skillName);


        // 원거리 ProjectileHitbox 연결
        var projectileHit = obj.GetComponentInChildren<ProjectileHitbox>();
        if (projectileHit != null)
        {
            projectileHit.Launch(position, rotation * Vector3.forward);

            if (onHit != null)
            {
                projectileHit.OnHit -= onHit;
                projectileHit.OnHit += onHit;
            }
        }

        // 스킬 종료 후 풀로 반환
        StartCoroutine(ReturnAfterSkill(obj, ps, pool));

        return obj;
    }


    private IEnumerator ReturnAfterSkill(GameObject obj, ParticleSystem ps, Queue<GameObject> pool)
    {
        if (ps != null)
        {
            while (ps.IsAlive(true))
                yield return null;
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}