using UnityEngine;

public class EnemyDummy : MonoBehaviour, IDamageableSkill
{
    public float maxHP = 100f;
    public float currentHP = 100f;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount, Vector3 hitPoint, Transform source)
    {
        currentHP -= amount;
        Debug.Log($"{name} 피해 {amount} → 남은 HP {currentHP}");

        if (currentHP <= 0f)
        {
            Debug.Log($"{name} 격파!");
            Destroy(gameObject);
        }
    }
}
