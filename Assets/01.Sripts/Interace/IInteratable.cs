using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable //확장가능 문,상자,NPC 등
{
    public string GetInteractPrompt();
    public void OnInteract();
}
