using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    private void Awake()
    {
        if(MapManager.Instance.GetComponent<DropItemSpawner>().dropItem == null)
        {
            MapManager.Instance.GetComponent<DropItemSpawner>().dropItem = this.gameObject;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }


    public string GetInteractPrompt()
    {
        return $"TakeItem";
    }

    public async void OnInteract()
    {
        if (BattleManager.Instance.currentMonster == null) return;
        await UIManager.Instance.Show<SelectAbilityUI>();
    }
}
