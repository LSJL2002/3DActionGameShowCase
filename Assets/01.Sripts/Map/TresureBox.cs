using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureBox : MonoBehaviour, IInteractable
{


    public string GetInteractPrompt()
    {
        return "Open";
    }

    public void OnInteract()
    {
        PlayerManager.Instance.Stats.TakeDamage(1000);
    }

}
