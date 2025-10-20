using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    public GameObject inventory; // 켜고 끌 대상

    private void Awake()
    {
        inventory.SetActive(false);
    }

    private void Update()
    {
    }
}