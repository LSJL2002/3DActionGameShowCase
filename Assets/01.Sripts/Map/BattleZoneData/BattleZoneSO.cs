using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattleZone", menuName = "BattleZone Data")]

public class BattleZoneSO : ScriptableObject
{
    public int id;
    public int MonsterId;
    public int[] nextZoneId;
}
