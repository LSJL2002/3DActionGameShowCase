using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Skill List", menuName = "Custom Data/Skill List")]
public class SkillListContainer : ScriptableObject
{
    public List<SkillDataEX> skills;
}