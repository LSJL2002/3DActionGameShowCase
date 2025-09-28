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
        public int poolSize = 5;
    }

    public SkillPrefab[] skillPrefabs;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new();

    private void Awake()
    {
        InitializePools();
    }

    /// <summary>
    /// 씬에 배치된 원본 오브젝트를 기준으로 풀 초기화
    /// </summary>
    private void InitializePools()
    {
        foreach (var skill in skillPrefabs)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < skill.poolSize; i++)
            {
                // 씬에서 배치한 원본을 복사
                var obj = Instantiate(skill.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDictionary.Add(skill.skillName, queue);
        }
    }

    private GameObject CreateSkillObject(GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }

    private GameObject CreateSkillPrefab(string skillName)
    {
        var skillPrefab = Array.Find(skillPrefabs, s => s.skillName == skillName);
        return skillPrefab != null ? CreateSkillObject(skillPrefab.prefab) : null;
    }

    /// <summary>
    /// 풀에서 사용 가능한 오브젝트 가져오기
    /// 이미 재생 중이면 새로운 인스턴스 생성
    /// </summary>
    private GameObject GetAvailableSkillObject(string skillName)
    {
        if (!poolDictionary.ContainsKey(skillName)) return null;

        var queue = poolDictionary[skillName];
        GameObject obj = null;

        if (queue.Count > 0)
        {
            obj = queue.Dequeue();

            // 이미 켜져있으면 새로 복제
            if (obj.activeSelf)
            {
                obj = Instantiate(skillPrefabs[Array.FindIndex(skillPrefabs, s => s.skillName == skillName)].prefab, transform);
            }
        }
        else
        {
            obj = Instantiate(skillPrefabs[Array.FindIndex(skillPrefabs, s => s.skillName == skillName)].prefab, transform);
        }
        return obj;
    }

    /// <summary>
    /// 스킬 발동: 매니저 자신의 현재 위치/회전에서 생성, 연속 사용 가능
    /// </summary>
    public GameObject SpawnSkill(string skillName)
    {
        var obj = GetAvailableSkillObject(skillName);
        if (obj == null) return null;

        obj.SetActive(true);

        // 파티클 재생
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        ps?.Play(true);

        // 히트박스 활성화
        var hitbox = obj.GetComponentInChildren<Hitbox>();
        hitbox?.OnEnable();

        // 사운드
        AudioManager.Instance?.PlaySFX(skillName);

        StartCoroutine(ReturnAfterParticle(ps, skillName, obj));
        return obj;
    }


    private IEnumerator ReturnAfterParticle(ParticleSystem ps, string skillName, GameObject obj)
    {
        if (ps != null)
        {
            while (ps.IsAlive(true))
                yield return null;

            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // Hitbox 끄기
        var hitbox = obj.GetComponentInChildren<Hitbox>();
        hitbox?.OnDisable();

        obj.SetActive(false);

        // 다시 풀에 넣기
        if (poolDictionary.ContainsKey(skillName))
            poolDictionary[skillName].Enqueue(obj);
    }
}