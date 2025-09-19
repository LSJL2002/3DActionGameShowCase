using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return $"TakeItem";
    }

    public async void OnInteract()
    {
        await UIManager.Instance.Show<SelectAbilityUI>();
        gameObject.SetActive(false);
        BattleManager.Instance.ClearBattle();
    }
}
