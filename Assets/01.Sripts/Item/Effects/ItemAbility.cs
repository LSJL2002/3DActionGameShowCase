using UnityEngine;

public abstract class ItemAbility : ScriptableObject
{
    // 아이템을 사용했을 때 실행될 로직을 정의하는 추상 메서드
    public abstract void Use(ItemData itemData);
}