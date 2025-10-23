using UnityEngine;
using UnityEngine.TextCore.Text;
using System;

public static class PlayerSwapService
{
    // 캐릭터 스왑 처리
    public static void Swap(PlayerManager manager, int newIndex)
    {
        var oldChar = manager.ActiveCharacter;
        var newChar = manager.Characters[newIndex];

        if (newChar.Ability.IsDeath || oldChar == newChar) return;

        // 위치 / 회전 / 활성화
        newChar.transform.position = oldChar.transform.position + oldChar.transform.right * 0.8f;
        newChar.transform.rotation = oldChar.transform.rotation;
        if (!newChar.gameObject.activeSelf) newChar.gameObject.SetActive(true);

        // Ability, Camera
        newChar.Ability.StartSwapIn();
        manager.Camera?.SetPlayerTarget(newChar.transform, newChar.Face);

        // Manager 업데이트
        manager.CurrentIndex = newIndex;
        manager.NotifyActiveCharacterChanged(newChar);
    }
}