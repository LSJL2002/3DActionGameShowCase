using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "ModuleData_Yuki", menuName = "Characters/Yuki")]
public class ModuleData_Yuki : ModuleDataBase
{
    [SerializeField] public float AwakenGaugeCost = 50f;
    [SerializeField] public float MovementSpeedModifier = 1.2f;

    // SO가 로드되거나 Inspector에서 선택될 때 자동 호출
}