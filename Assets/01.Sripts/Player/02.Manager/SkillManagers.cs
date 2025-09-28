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
    }

    public SkillPrefab[] skillPrefabs;

    /// <summary>
    /// 스킬 이름 → 오브젝트 맵 (풀링 없이 단일 인스턴스)
    /// </summary>
    private readonly Dictionary<string, GameObject> skillObjects
        = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // 각 스킬 오브젝트 씬에 배치
        foreach (var skill in skillPrefabs)
        {
            if (skill.prefab == null) continue;

            var obj = Instantiate(skill.prefab, transform);
            obj.SetActive(false);
            skillObjects[skill.skillName] = obj;
        }
    }


    /// <summary>
    /// 스킬 발동: 단일 오브젝트 껐다 켰다
    /// </summary>
    public GameObject SpawnSkill(string skillName)
    {
        if (!skillObjects.TryGetValue(skillName, out var obj) || obj == null)
            return null;

        obj.SetActive(true);

        // 파티클 재생
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        ps?.Play(true);

        // 히트박스 활성화
        var hitbox = obj.GetComponentInChildren<Hitbox>();
        hitbox?.OnEnable();

        // 사운드
        AudioManager.Instance?.PlaySFX(skillName);

        StartCoroutine(ReturnAfterParticle(ps, obj));

        return obj;
    }

    private IEnumerator ReturnAfterParticle(ParticleSystem ps, GameObject obj)
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

        // 오브젝트 비활성화
        obj.SetActive(false);
    }
}