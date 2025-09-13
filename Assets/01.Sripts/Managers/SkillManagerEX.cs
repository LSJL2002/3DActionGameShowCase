using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManagerEX : Singleton<SkillManagerEX>
{
    // 모든 스킬을 고유한 ID를 키로 하여 딕셔너리로 관리
    private Dictionary<string, SkillDataEX> skillDictionary = new Dictionary<string, SkillDataEX>();

    // 스킬 리스트 컨테이너 에셋을 인스펙터에서 할당
    [SerializeField] private SkillListContainer skillListContainer;

    // float : 쿨타임
    // string : 스킬이름(딕셔너리 키)
    public static event Action<float, string> OnSkillUsed;

    // 모든 스킬 목록을 딕셔너리 형태로 반환
    public Dictionary<string, SkillDataEX> GetSkillDictionary()
    {
        return skillDictionary;
    }

    // 특정 키(스킬 ID)에 해당하는 SkillData를 반환
    public SkillDataEX GetSkillData(string key)
    {
        if (skillDictionary.TryGetValue(key, out SkillDataEX skillData))
        {
            return skillData;
        }
        return null;
    }

    protected override void Start()
    {
        base.Start();

        // 스킬 데이터를 로드 및 딕셔너리에 추가하는 로직 추가

        // 딕셔너리에 스킬 추가
        LoadSkillsFromContainer();
    }

    // SkillListContainer의 데이터를 딕셔너리에 로드하는 함수
    private void LoadSkillsFromContainer()
    {
        // 컨테이너에 있는 모든 스킬 데이터를 딕셔너리에 추가
        foreach (var skillData in skillListContainer.skills)
        {
            // 이미 있는지 체크
            if (skillDictionary.ContainsKey(skillData.skillID))
            {
                Debug.LogWarning($"스킬 ID '{skillData.skillID}'가 이미 존재합니다. 스킬 이름: {skillData.skillName}");
                continue;
            }

            // skillID를 키로, SkillDataEX 객체를 값으로 딕셔너리에 추가
            skillDictionary.Add(skillData.skillID, skillData);
        }

        Debug.Log($"총 {skillDictionary.Count}개의 스킬이 로드되었습니다.");
    }

    public void UseSkill(float cooltime, string skillID)
    {
        Debug.Log($"{skillID} 스킬 사용");
        
        OnSkillUsed?.Invoke(cooltime, skillID);
    }
}
