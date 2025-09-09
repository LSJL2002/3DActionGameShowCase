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
    //public StatHandler Stats {get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTakeDamage(int amount)
    {

    }
}
