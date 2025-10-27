using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleDataBase : ScriptableObject
{
    [SerializeField] protected List<AttackInfoData> attackInfoDatas = new List<AttackInfoData>();
    public List<AttackInfoData> AttackInfoDatas => attackInfoDatas;
}