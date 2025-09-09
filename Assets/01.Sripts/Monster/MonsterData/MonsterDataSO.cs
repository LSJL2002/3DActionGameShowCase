using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    public string ID;
    public string 이름;
    public int 최대체력;
    public int 최대엠피;
    public int 공격력;
    public int 방어력;
    public float 공격속도;
    public List<string> 상태이상;
    public float 이동속도;
    public int 장비한무기;
    public int 장비한방어구;
    public int 장비한악세사리;
    public List<int> 드랍아이템;
    public int 인식범위;
    public int 공격범위;
}
