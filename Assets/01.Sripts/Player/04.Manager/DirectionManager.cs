using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory; // 켜고 끌 대상
    private bool isActive;

    private PlayerManager player;

    private void Awake()
    {
        player ??= GetComponentInParent<PlayerManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;
            inventory.SetActive(isActive);

            player.EnableInput(!isActive);
        }
    }
}