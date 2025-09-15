using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManagerEX : Singleton<SkillManagerEX>
{
    // 스킬 리스트 컨테이너 에셋을 인스펙터에서 할당
    [SerializeField] private SkillListContainer skillListContainer;
    
    // 모든 스킬을 고유한 ID를 키로 하여 딕셔너리로 관리
    private Dictionary<string, SkillDataEX> skillDictionary = new Dictionary<string, SkillDataEX>();

    // 스킬아이콘UI에 알릴 이벤트 변수 선언
    // float : 쿨타임
    // string : 스킬이름(딕셔너리 키)
    public static event Action<float, string> OnSkillUsed;

    protected override void Start()
    {
        base.Start();

        LoadSkillsFromContainer(); // 딕셔너리에 스킬 추가

        StartCoroutine(OnSkill(10)); // 스킬 테스트
    }

    // 스킬쿨타임 체크용 코루틴 추가
    private IEnumerator OnSkill(int sec)
    {
        yield return new WaitForSeconds(sec);

        // 테스트 스킬1 발동
        SkillDataEX skillData = GetSkillData("1");
        float cooltime = skillData.cooltime;
        string skillID = skillData.skillID;

        UseSkill(cooltime, skillID);
    }

    // SkillListContainer의 데이터를 딕셔너리에 로드하는 함수
    private void LoadSkillsFromContainer()
    {
        foreach (var skillDataEX in skillListContainer.skills)
        {
            // 이미 있는지 체크
            if (skillDictionary.ContainsKey(skillDataEX.skillID))
            {
                Debug.LogWarning($"스킬 ID '{skillDataEX.skillID}'가 이미 존재합니다. 스킬 이름: {skillDataEX.skillName}");
                continue;
            }

            // skillID를 키로, SkillDataEX 객체를 값으로 딕셔너리에 추가
            skillDictionary.Add(skillDataEX.skillID, skillDataEX);
        }
    }

    // 모든 스킬 목록을 딕셔너리 형태로 반환 할 경우 사용
    public Dictionary<string, SkillDataEX> GetSkillDictionary()
    {
        return skillDictionary;
    }

    // 특정 키(스킬 ID)에 해당하는 SkillData를 반환할 경우 사용
    public SkillDataEX GetSkillData(string key)
    {
        if (skillDictionary.TryGetValue(key, out SkillDataEX skillDataEX))
        {
            return skillDataEX;
        }
        return null;
    }

    // 스킬 사용 함수
    public void UseSkill(float cooltime, string skillID)
    {
        Debug.Log($"{skillID} 스킬 사용! (쿨타임:{cooltime})");
        
        // 스킬 사용 이벤트 발생
        OnSkillUsed?.Invoke(cooltime, skillID);
    }
}
