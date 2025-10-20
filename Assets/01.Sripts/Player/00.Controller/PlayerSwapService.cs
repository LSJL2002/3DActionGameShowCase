using UnityEngine;
using UnityEngine.TextCore.Text;
using System;

public static class PlayerSwapService
{
    public static void PrepareCharacterSwap(PlayerCharacter oldChar, PlayerCharacter newChar)
    {
        newChar.transform.position = oldChar.transform.position + oldChar.transform.right * 0.8f;
        newChar.transform.rotation = oldChar.transform.rotation;

        if (!newChar.gameObject.activeSelf)
            newChar.gameObject.SetActive(true);
    }
}