using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    private string dropItemText;
    private bool isInteracted = true;

    private void Start()
    {
        dropItemText = $"Press Key.F";
        if (MapManager.Instance != null)
        {
            if (MapManager.Instance.GetComponent<DropItemSpawner>().dropItem == null)
            {
                MapManager.Instance.GetComponent<DropItemSpawner>().dropItem = this.gameObject;
            }
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        dropItemText = $"Press Key.F";
        isInteracted = true;
    }


    public string GetInteractPrompt()
    {
        return dropItemText;
    }

    public async void OnInteract()
    {
        if(!isInteracted) return;
        isInteracted = false;

        dropItemText = string.Empty;
        if (BattleManager.Instance.currentMonster == null) return;
        await UIManager.Instance.Show<SelectAbilityUI>();
    }
}
