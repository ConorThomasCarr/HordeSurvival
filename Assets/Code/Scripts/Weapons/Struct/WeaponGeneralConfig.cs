using System;
using UnityEngine;

[Serializable]
public struct WeaponGeneralConfig
{
    public GameObject Parent {get; set;}
    public Transform MuzzleOne {get;set;}
    public Transform MuzzleTwo {get;set;}
    public int MaxMagAmmo {get;set;}
    public int MaxAmmoCapacity  {get;set;}

    public WeaponGeneralConfig(GameObject parent, Transform muzzleOne, Transform muzzleTwo, int maxMagAmmo, int maxAmmoCapacity)
    {
        Parent = parent;
        MuzzleOne = muzzleOne;
        MuzzleTwo = muzzleTwo;
        MaxMagAmmo = maxMagAmmo;
        MaxAmmoCapacity = maxAmmoCapacity;
    }
}
