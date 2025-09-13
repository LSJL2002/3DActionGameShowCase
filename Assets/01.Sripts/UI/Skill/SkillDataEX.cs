using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Custom Data/Skill Data")]
public class SkillDataEX : ScriptableObject
{
    public string skillID; // 딕셔너리의 키로 사용될 고유 ID
    public float cooltime;
    public string skillName;
}