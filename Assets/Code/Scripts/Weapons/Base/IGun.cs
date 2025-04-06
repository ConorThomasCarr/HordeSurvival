using UnityEngine;
using UnityEngine.Events;

namespace Weapon.BaseGun
{
    public interface IGun
    {
        WeaponConfig weaponConfig {get; set;}
    
        float WeaponSpread {get; set;}
    
        float AimAngle {get; set;}
    
        void InitializeEvents();

        void InitializeConfig(WeaponConfig weaponData);
    
        void Shoot();
    
        void Aim();

        void Reload();
    
        void HasAim();

        public void HasNoAim();
    
        bool CanShoot();
    
        bool CanAim();

        bool IsAiming();

        Mesh AimMesh(Transform transform);
    }
}


    

