using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/AllPatterns")]
public class MonsterAllPatternsSO : ScriptableObject
{
    [System.Serializable]
    public class PatternEntry
    {
        public int id;
        public List<string> states;
    }

    public List<PatternEntry> patterns = new List<PatternEntry>();

    public PatternEntry GetPatternById(int id)
    {
        return patterns.Find(p => p.id == id);
    }
}
