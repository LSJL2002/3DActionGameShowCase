using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEnenmy : MonoBehaviour
{
    private IStats target; // PlayerStats가 들어올 수 있음

    public EEnenmy(IStats target) //생성자강제주입 > 이적은 반드시 공격할 대상을 알아야만 만들어질수 있다
    {
        this.target = target;
    }

    public void Attack()
    {
        target.TakeDamage(10f); // PlayerStats 내부 구현 몰라도 사용 가능
    }


    private readonly float healAmount;

    public EEnenmy(float healAmount)
    {
        this.healAmount = healAmount;
    }

    public void Use(IStats target)
    {
        target.RecoverHealth(healAmount);
    }
}
