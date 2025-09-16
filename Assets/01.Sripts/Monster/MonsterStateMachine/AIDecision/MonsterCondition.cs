using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterCondition
{
    public int id; // 조건 ID
    [Range(0, 100)]
    public float minHpPercent;
    [Range(0, 100)]
    public float maxHpPerecent;
    public List<int> possiblePatterns; // 해당 조건에서 선택 가능한 패턴 ID들
}