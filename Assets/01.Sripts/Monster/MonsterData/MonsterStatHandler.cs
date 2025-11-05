using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MonsterStatHandler : MonoBehaviour
{
    [Header("Template Data (SO)")]
    public MonsterSO monsterData;

    [Header("Runtime Stats (Inspector창 변경)")]
    public float maxHp;
    public float maxMp;
    public float CurrentHP;
    public float CurrentMP;
    public int AttackPower;
    public int Defense;
    public float AttackSpeed;
    public float MoveSpeed;
    public int DetectRange;
    public int AttackRange;
    public List<string> StatusEffect;
    public List<int> DropItem;
    public List<MonsterSkillSO> MonsterSkills;

    private Dictionary<string, MonsterSkillSO> skillDict;
    // 현재 다이얼로그 임계값
    private float currentDialogueThreshold = 0.90f;

    // 다이얼로그 임계값 목록
    private readonly float[] dialogueThresholds = { 0.90f, 0.60f, 0.30f, 0.10f };



    void Awake()
    {
        if (monsterData != null)
        {
            maxHp = monsterData.maxHp;
            CurrentHP = maxHp;
            maxMp = monsterData.maxMp;
            CurrentMP = maxMp;
            AttackPower = monsterData.attackPower;
            Defense = monsterData.defense;
            AttackSpeed = monsterData.attackSpeed;
            MoveSpeed = monsterData.moveSpeed;
            DetectRange = monsterData.detectRange;
            AttackRange = monsterData.attackRange;
            StatusEffect = new List<string>(monsterData.statusEffect);

            MonsterSkills = new List<MonsterSkillSO>(monsterData.useSkill);

            skillDict = new Dictionary<string, MonsterSkillSO>();
            foreach (var skill in MonsterSkills)
            {
                if (skill != null && !skillDict.ContainsKey(skill.skillName))
                {
                    skillDict.Add(skill.skillName, skill);
                }
            }
        }
    }


    public MonsterSkillSO GetSkill(string skillName)
    {
        if (skillDict != null && skillDict.TryGetValue(skillName, out var skill))
            return skill;
        return null;
    }

    public void UpdateHealthUI()
    {
        EventsManager.Instance.TriggerEvent(GameEvent.OnHealthChanged);
        if (monsterData.id == 10000001)
        {
            CheckHealthThresholds();
        }
    }

    public void Die()
    {
        Debug.Log($"{monsterData.monsterName} 사망!");
    }

    private void CheckHealthThresholds()
    {
        float hpPercent = CurrentHP / maxHp;

        // 현재 임계값에 도달했는지 확인
        if (hpPercent <= currentDialogueThreshold)
        {
            CallDialogue();

            // 다음 임계값으로 업데이트
            UpdateDialogueThreshold();
        }
    }
    private void UpdateDialogueThreshold()
    {
        // 현재 임계값의 인덱스를 찾고 다음 임계값으로 이동
        int currentIndex = Array.IndexOf(dialogueThresholds, currentDialogueThreshold);
        if (currentIndex < dialogueThresholds.Length - 1)
        {
            currentDialogueThreshold = dialogueThresholds[currentIndex + 1];
        }
        else
        {
            // 더 이상 임계값이 없으면 무효화
            currentDialogueThreshold = -1f;
        }
    }

    public async void CallDialogue()
    {
        // 현재 체력 비율 계산
        float hpPercent = (CurrentHP / maxHp);
        await UIManager.Instance.Show<TutorialUI>();
        UIManager.Instance.Get<TutorialUI>().TryPlayBossThresholdDialogue(SceneType.Boss_1, hpPercent);

    }
}
