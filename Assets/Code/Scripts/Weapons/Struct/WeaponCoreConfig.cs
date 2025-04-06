using System;
using UnityEngine;

[Serializable]
public struct WeaponCoreConfig 
{
    public float WeaponRangeMaxAngle {get; set;}
    public float WeaponRangeMinAngle{get; set;}
    
    public float WeaponSpreadMaxAngle {get; set;}
    
    public float WeaponSpreadMinAngle{get; set;}
    public float weaponMaxSpread {get; set;}
    public float weaponMinSpread {get; set;}
    public float reloadCooldown {get; set;}
    public float swapWeaponCooldown {get; set;}
    
    public int magazineSize {get; set;}
    public int magazineCapacity {get; set;}
    public int maxAmmoCapacity {get; set;}
 
    
    public WeaponCoreConfig (float weaponRangeMaxAngle, float weaponRangeMinAngle, float weaponSpreadMaxAngle,float weaponSpreadMinAngle, float _weaponMaxSpread, float _weaponMinSpread, 
        float _reloadCooldown, float _swapWeaponCooldown, int _magazineSize, int _magazineCapacity, int _maxAmmoCapacity)
    {
        WeaponRangeMaxAngle = weaponRangeMaxAngle;
        WeaponRangeMinAngle = weaponRangeMinAngle;
        WeaponSpreadMaxAngle = weaponSpreadMaxAngle;
        WeaponSpreadMinAngle = weaponSpreadMinAngle;
        
        weaponMaxSpread = _weaponMaxSpread;
        weaponMinSpread = _weaponMinSpread;
        reloadCooldown = _reloadCooldown;
        swapWeaponCooldown = _swapWeaponCooldown;
        
        magazineSize  = _magazineSize;
        magazineCapacity = _magazineCapacity;
        maxAmmoCapacity = _maxAmmoCapacity;
        
    }
}
