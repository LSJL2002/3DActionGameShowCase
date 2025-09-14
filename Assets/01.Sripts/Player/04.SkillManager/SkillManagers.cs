using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagers : Singleton<SkillManagers>
{
    [Serializable]
    public class SkillPrefab
    {
        public string skillName;
        public GameObject prefab;
        public int poolSize = 10;
    }

    public SkillPrefab[] skillPrefabs;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new();

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var skill in skillPrefabs)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < skill.poolSize; i++)
            {
                var obj = Instantiate(skill.prefab);
                obj.transform.SetParent(transform, false); // 로컬 좌표 유지
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

    public GameObject SpawnSkill(string skillName)
    {
        if (!poolDictionary.ContainsKey(skillName))
        {
            Debug.LogWarning($"Skill '{skillName}' not found in pool!");
            return null;
        }

        var queue = poolDictionary[skillName];
        GameObject obj = queue.Count > 0 ? queue.Dequeue() : CreateSkillPrefab(skillName);
        if (obj == null) return null;

        // owner를 부모로 설정, prefab에서 세팅한 local 위치/회전 유지
        obj.transform.SetParent(transform, false);

        obj.SetActive(true);

        // Hitbox 켜기
        var hitbox = obj.GetComponent<Hitbox>();
        hitbox?.OnEnable();

        // ParticleSystem 재생
        var ps = obj.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play(true);

        // 오디오 재생 (AudioManager 사용)
        AudioManager.Instance.PlaySFX(skillName);

        // Coroutine으로 반환 처리
        StartCoroutine(ReturnAfterParticle(ps, skillName, obj));

        return obj;
    }

    private GameObject CreateSkillPrefab(string skillName)
    {
        var skillPrefab = Array.Find(skillPrefabs, s => s.skillName == skillName);
        if (skillPrefab == null) return null;
        return CreateSkillObject(skillPrefab.prefab);
    }

    private IEnumerator ReturnAfterParticle(ParticleSystem ps, string skillName, GameObject obj)
    {
        if (ps != null)
        {
            // Particle이 완전히 끝날 때까지 기다림
            while (ps.IsAlive(true))
                yield return null;

            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // Hitbox 끄기
        var hitbox = obj.GetComponent<Hitbox>();
        hitbox?.OnDisable();

        obj.SetActive(false);

        // Pool에 다시 넣기
        if (poolDictionary.ContainsKey(skillName))
            poolDictionary[skillName].Enqueue(obj);
    }
}
