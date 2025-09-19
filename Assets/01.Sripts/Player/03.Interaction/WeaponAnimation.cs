using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [SerializeField] private GameObject handWeapon;
    [SerializeField] private GameObject holsterWeapon;
    [SerializeField] private ParticleSystem spawnFX;
    [SerializeField] private ParticleSystem despawnFX;

    public void EquipWeapon()
    {
        holsterWeapon.SetActive(false);
        handWeapon.SetActive(true);

        if (spawnFX != null)
        {
            spawnFX.Play();
        }
    }

    public void HolsterWeapon()
    {
        if (despawnFX != null)
        {
            despawnFX.Play();
        }

        handWeapon.SetActive(false);
        holsterWeapon.SetActive(true);
    }
}