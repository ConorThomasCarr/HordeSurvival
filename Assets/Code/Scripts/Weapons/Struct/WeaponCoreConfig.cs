using System;
using UnityEngine;

[Serializable]
public struct WeaponCoreConfig 
{
    
    public float WeaponRangeMaxAngle {get; set;}
    public float WeaponRangeMinAngle{get; set;}
 
    public float WeaponSpreadMaxAngle {get; set;}
    public float WeaponSpreadMinAngle{get; set;}
 
    public float reloadCooldown {get; set;}
    public float swapWeaponCooldown {get; set;}
 
    public int magazineCapacity {get; set;}
    public int maxAmmoCapacity {get; set;}
    
    public float spreadSpeed {get; set;}
    public float aimSpeed {get; set;}
    
    public WeaponCoreConfig (float weaponRangeMaxAngle, float weaponRangeMinAngle, 
        float weaponSpreadMaxAngle,float weaponSpreadMinAngle, float _reloadCooldown, 
        float _swapWeaponCooldown, int _magazineCapacity, int _maxAmmoCapacity,
        float _spreadSpeed, float _aimSpeed)
    {
        WeaponRangeMaxAngle = weaponRangeMaxAngle;
        WeaponRangeMinAngle = weaponRangeMinAngle;
        WeaponSpreadMaxAngle = weaponSpreadMaxAngle;
        WeaponSpreadMinAngle = weaponSpreadMinAngle;
        
        reloadCooldown = _reloadCooldown;
        swapWeaponCooldown = _swapWeaponCooldown;
        
        magazineCapacity = _magazineCapacity;
        maxAmmoCapacity = _maxAmmoCapacity;
        
        spreadSpeed = _spreadSpeed;
        aimSpeed = _aimSpeed;
    }
}
