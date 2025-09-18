using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHit : MonoBehaviour, IDamageable
{
    public virtual void OnTakeDamage(int amount)
    {
        Debug.Log("object is hit");
    }
}
