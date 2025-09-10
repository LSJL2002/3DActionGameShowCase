using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField]
    private bool isBattle;
    [SerializeField]
    private bool isClear;

    public GameObject[] walls;
    public Transform spawnPoint;

    private void Start()
    {
        foreach (GameObject wall in walls)
        {
            wall.SetActive(false);
        }
    }

    public static event Action<BattleZone> OnBattleClear;
    public static event Action<BattleZone> OnBattle;

    private void Update()
    {
        if (isClear)
        {
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isClear = !isClear;
            OnBattleClear?.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!isClear)
        {
            isBattle = true;
            Debug.Log("전투가 시작됩니다.");

            OnBattle?.Invoke(this);

            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
                Debug.Log($"{wall.name}이 켜졌습니다.");
            }
        }

        
    }

    
}
