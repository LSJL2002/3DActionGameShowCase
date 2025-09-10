using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class BaseMonster : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] public MonsterAnimationData animationData;
    
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public MonsterStatHandler Stats {get; private set; }

    public Transform EnemeyTarget { get; set; }
    //

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void OnTakeDamage(int amount)
    {
        Stats.CurrentHP -= 10;
        if (Stats.CurrentHP <= 0)
        {
            Stats.Die();
        }
    }
}
