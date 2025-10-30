using System.Collections.Generic;
using UnityEngine;

public enum States
{
    BaseAttack,
    BaseAttack2,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Skill5,
    TurnLeft,
    TurnRight
}

public enum ConditionType
{
    HpRange,
    HpThreshold,   // replaces HpEquals
    DistanceCheck
}

[CreateAssetMenu(menuName = "Monster/PatternConfig")]
public class MonsterPatternSO : ScriptableObject
{
    [System.Serializable]
    public class PatternEntry
    {
        public int id;
        public List<States> states;
    }

    [System.Serializable]
    public class PatternCondition
    {
        public int id;
        public ConditionType conditionType;

        [Range(0, 100)] public float minHpPercent;
        [Range(0, 100)] public float maxHpPercent;

        [Range(0, 100)] public float thresholdHpPercent;
        public float requiredDistance;
        public bool triggerOnce;
        [HideInInspector] public bool hasTriggered = false;
        public List<int> possiblePatternIds;
        public int priority;
        public bool ignoreDistanceCheck;

        public float effectValueMultiplier = 1f;
        public float rangeMultiplier = 1f;
        public float preCastTimeMultiplier = 1f;
    }

    public List<PatternEntry> patterns = new List<PatternEntry>();
    public List<PatternCondition> conditions = new List<PatternCondition>();

    public PatternEntry GetPatternById(int id) =>
        patterns.Find(p => p.id == id);

    public List<PatternCondition> GetValidConditions(float currentHpPercent, float distanceToPlayer, bool hasStartedCombat)
    {
        var valid = new List<PatternCondition>();

        foreach (var cond in conditions)
        {
            switch (cond.conditionType)
            {
                case ConditionType.HpRange:
                    if (currentHpPercent <= cond.maxHpPercent && currentHpPercent > cond.minHpPercent)
                        valid.Add(cond);
                    break;

                case ConditionType.HpThreshold:
                    if (!cond.hasTriggered && currentHpPercent <= cond.thresholdHpPercent)
                    {
                        valid.Add(cond);
                        if (!cond.triggerOnce)
                            cond.hasTriggered = true;
                    }
                    break;

                case ConditionType.DistanceCheck:
                    if (hasStartedCombat && distanceToPlayer >= cond.requiredDistance)
                        valid.Add(cond);
                    break;
            }
        }

        // sort by priority (lowest number = higher priority)
        valid.Sort((a, b) => a.priority.CompareTo(b.priority));
        foreach (var cond in valid)
        {
            //Debug.Log($"Valid condition ID={cond.id}, priority={cond.priority}");
        }
        return valid;
    }
}
