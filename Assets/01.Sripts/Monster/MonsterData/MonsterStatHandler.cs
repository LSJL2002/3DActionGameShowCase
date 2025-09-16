using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MonsterStatHandler : MonoBehaviour
{
    [Header("Template Data (SO)")]
    public MonsterSO monsterData;

    [Header("Runtime Stats (Inspector창 변경)")]
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

    void Awake()
    {
        if (monsterData != null)
        {
            CurrentHP = monsterData.maxHp;
            CurrentMP = monsterData.maxMp;
            AttackPower = monsterData.attackPower;
            Defense = monsterData.defense;
            AttackSpeed = monsterData.attackSpeed;
            MoveSpeed = monsterData.moveSpeed;
            DetectRange = monsterData.detectRange;
            AttackRange = monsterData.attackRange;
            StatusEffect = new List<string>(monsterData.statusEffect);

            skillDict = new Dictionary<string, MonsterSkillSO>();
            foreach (var skill in MonsterSkills)
            {
                if (!skillDict.ContainsKey(skill.skillName))
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

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, monsterData.maxHp);
    }

    public bool isAlive()
    {
        if (CurrentHP > 0)
        {
            return true;
        }
        return false;
    }

    public void Die()
    {
        Debug.Log($"{monsterData.monsterName} 사망!");
        //Destroy(gameObject);
    }
}
