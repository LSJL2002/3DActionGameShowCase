using UnityEngine;

/// <summary>
/// 플레이어의 MP와 공격력 관리
/// </summary>
public class PlayerStatsSkill : MonoBehaviour
{
    [Header("전투 수치")]
    public float attackPower = 10f;
    public float maxMP = 100f;
    public float currentMP = 100f;

    public bool TryConsumeMP(float cost)
    {
        if (currentMP < cost) return false;
        currentMP -= cost;
        return true;
    }
}
