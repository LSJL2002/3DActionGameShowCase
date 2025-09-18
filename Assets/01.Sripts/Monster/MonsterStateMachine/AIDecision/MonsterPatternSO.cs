using System.Collections.Generic;
using UnityEngine;

public enum States
{
    BaseAttack,
    BaseAttack2,
    Skill1,
    Skill2,
    Skill3,
    Skill4

}

[CreateAssetMenu(menuName = "Monster/AllPatterns")]
public class MonsterAllPatternsSO : ScriptableObject
{
    [System.Serializable]
    public class PatternEntry
    {
        public int id;
        public List<States> states;
    }

    public List<PatternEntry> patterns = new List<PatternEntry>();

    public PatternEntry GetPatternById(int id)
    {
        return patterns.Find(p => p.id == id);
    }
}
