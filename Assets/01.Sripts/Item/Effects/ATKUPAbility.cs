using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAbility", menuName = "Item Abilities/ATKUP")]
public class ATKUPAbility : ItemAbility
{
    public override void Use(ItemData itemData)
    {
        ItemManager.Instance.UseItem(itemData);
    }
}