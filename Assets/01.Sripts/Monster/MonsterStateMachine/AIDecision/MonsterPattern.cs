using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPattern
{
    public int id; // 패턴 ID
    public List<MonsterSkillSO> skillIds; // 스킬 시퀀스
}
