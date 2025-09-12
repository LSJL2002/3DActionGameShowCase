using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsData : ScriptableObject
{
    public int id;
    public string name;
    public float maxHp;
    public float maxMp;
    public int attackPower;
    public int defense;
    public float attackSpeed;
    //public List<StatusEffect> statusEffectList = new List<StatusEffect>();
    public float moveSpeed;
    //public List<EquipWeaponId> = new List<EquipWeaponId>();
    //public List<equipArmorId> = new List<equipArmorId>();
    //public List<equipAccId> = new List<equipAccId>();
    public int jumpCount;
    public int dodgeCount;
    //public List<inventoryItemList> = new List<inventoryItemList>();
    //public List<effectItemList> = new List<effectItemList>();
    public int usableSkillDicList;
}
