using UnityEngine;

/// <summary>
/// 적/타겟이 반드시 구현해야 하는 인터페이스
/// </summary>
public interface IDamageableSkill
{
    void TakeDamage(float amount, Vector3 hitPoint, Transform source);
}
