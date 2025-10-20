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
        PlayerManager.Instance.Attr.Resource.TakeDamage(1000);
    }

}
