using UnityEngine;
using UnityEngine.Events;
using Weapon.BaseGun;

namespace Weapon.BaseWeapon
{
    public interface IWeapon
    {
        WeaponConfig WeaponData { get; set; } 
        
        IGun WeaponInterface {get; set;}
    
        UnityAction InitializeConstruction { get; set; }
    
        UnityAction<IGun> InitializeGun { get; set; }
    
        UnityAction InitializeConfigs { get; set; }
    
        UnityAction InitializeWeaponAction { get; set; }
        
        UnityAction UninitializeWeaponAction { get; set; }
    
        UnityAction Shoot { get; set; }
        
        UnityAction AimIsInProgress {get; set;}
        
        UnityAction AimReleased {get; set;}
        
        UnityAction Reload {get; set;}
    
        void Enable();
    
        void Disable();
    }
}


