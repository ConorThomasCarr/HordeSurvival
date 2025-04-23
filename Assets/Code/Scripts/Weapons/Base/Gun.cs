using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Weapon.BaseGun;

namespace Weapon.BaseWeapon.Gun
{
    public abstract class Gun : MonoBehaviour, IWeapon
    {
        public WeaponConfig WeaponConfig {get; set;}
        
        public IGun WeaponInterface {get; set;}
    
        public UnityAction InitializeConstruction { get; set; }
        
        public UnityAction <IGun> InitializeGun { get; set; }
      
        public UnityAction  InitializeConfigs { get; set; } 
    
        public UnityAction InitializeWeaponAction { get; set; }
    
        public UnityAction UninitializeWeaponAction { get; set; }
    
        public UnityAction Shoot {get; set;}
    
        public UnityAction AimIsInProgress {get; set;}
    
        public UnityAction AimReleased {get; set;}
        
        public UnityAction Reload {get; set;}

        public virtual void Enable() { }

        public virtual void Disable() { }
    }
}




