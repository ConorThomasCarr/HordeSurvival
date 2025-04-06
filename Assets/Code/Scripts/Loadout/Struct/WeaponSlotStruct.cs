using System;
using UnityEngine;
using Weapon.BaseWeapon;

[Serializable]
public struct WeaponSlotStruct
{
    public IWeapon gun;
    public WeaponConfig weaponData;
    public GameObject obj;
}
