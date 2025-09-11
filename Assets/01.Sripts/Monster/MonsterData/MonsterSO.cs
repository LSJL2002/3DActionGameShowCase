using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster Data")]
public class MonsterSO : ScriptableObject
{
    public int id;
    public string monsterName;
    public float maxHp;
    public float maxMp;
    public int attackPower;
    public int defense;
    public float attackSpeed;
    public List<string> statusEffect;
    public float moveSpeed;
    public int equipWeaponId;
    public int equipArmorId;
    public int equipAccId;
    public List<int> dropItem;
    public int detectRange;
    public int attackRange;
    //public List<int> useSkill; //나중에 추가
}
