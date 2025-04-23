using UnityEngine;
using UnityEngine.Events;

namespace Weapon.BaseGun
{
    public interface IGun
    {
        WeaponConfig WeaponConfig {get; set;}
    
        float WeaponSpread {get; set;}
    
        float AimAngle {get; set;}
        
        int magazineSize {get; set;}
        
        int maxAmmo {get; set;} 
        
        bool isReloading {get; set;}
    
        void InitializeEvents();

        void InitializeConfig(WeaponConfig weaponConfig);
    
        void Shoot();
    
        void Aim();

        void Reload();
    
        void HasAim();

        public void HasNoAim();
        
        bool CanAim();
        
        bool IsAiming();
        
        bool CanShoot();
        
        bool CanReload();
        
        Mesh AimMesh(Transform transform);
    }
}


    

