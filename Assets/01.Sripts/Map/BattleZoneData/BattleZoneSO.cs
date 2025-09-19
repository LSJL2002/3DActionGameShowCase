using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattleZone", menuName = "BattleZone Data")]

public class BattleZoneSO : ScriptableObject
{
    public int id;
    public string stageName;
    public int summonMonsterId;
    public List<int> moveAbleStage;
    public List<int> getableItemTable;
}
//public int id;
//public int MonsterId;
//public int[] nextZoneId;