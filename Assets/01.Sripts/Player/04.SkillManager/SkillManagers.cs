using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagers : Singleton<SkillManagers>
{
    [System.Serializable]
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
                obj.SetActive(false);
                obj.transform.parent = transform;
                queue.Enqueue(obj);
            }
            poolDictionary.Add(skill.skillName, queue);
        }
    }

    public GameObject SpawnSkill(string skillName, Transform owner)
    {
        if (!poolDictionary.ContainsKey(skillName)) return null;

        var queue = poolDictionary[skillName];
        GameObject obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(skillPrefabs[0].prefab);

        // 스킬 위치/회전 → 소환자(owner) 기준
        obj.transform.position = owner.position;
        obj.transform.rotation = owner.rotation;
        obj.SetActive(true);

        // Hitbox 켜기
        var hitbox = obj.GetComponent<Hitbox>();
        hitbox?.OnEnable();

        // 파티클 실행
        var ps = obj.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play(true);

        // 일정 시간 후 반환
        float duration = ps != null ? ps.main.duration : 1.0f;
        StartCoroutine(ReturnAfterTime(skillName, obj, duration));

        return obj;
    }


    private IEnumerator ReturnAfterTime(string skillName, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        var hitbox = obj.GetComponent<Hitbox>();
        hitbox?.OnDisable();

        var ps = obj.GetComponent<ParticleSystem>();
        if (ps != null) ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        obj.SetActive(false);
        if (poolDictionary.ContainsKey(skillName))
            poolDictionary[skillName].Enqueue(obj);
    }
}
