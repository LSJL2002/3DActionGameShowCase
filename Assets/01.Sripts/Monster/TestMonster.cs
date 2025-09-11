using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : BaseMonster
{
    [Header("AttackIndicator")]
    public GameObject circleBorderPrefab;
    public GameObject circleFillPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

}
