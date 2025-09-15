using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Monster/PatternTable")]
public class MonsterPatternSO : ScriptableObject
{
    public List<MonsterPattern> patterns;
    public List<MonsterCondition> conditions;
}
