using UnityEngine;

namespace Weapon.BaseGun.BaseDoubleBarrelShotgun
{
    public abstract class BaseDoubleBarrelShotgun : IGun
    {
        public float WeaponSpread {get; set;}
    
        public float AimAngle {get; set;}
        
        public int magazineSize {get; set;}
        
        public int maxAmmo {get; set;}
        
        public bool isReloading {get; set;}
    
        public WeaponConfig WeaponConfig {get; set;}
        
        public virtual void InitializeEvents() { }

        public virtual void InitializeConfig(WeaponConfig weaponConfig) { this.WeaponConfig = weaponConfig; }
        
        public virtual void Shoot() { }
    
        public virtual void Aim() { }
        
        public virtual void Reload(){ }
    
        public virtual void HasAim(){ }
    
        public virtual void HasNoAim() { }
    
        public virtual bool CanAim() {return false;}
    
        public virtual  bool IsAiming() { return false; }
        
        public virtual bool CanShoot() { return false; }
        
        public virtual bool CanReload() { return false; }
        
        public virtual Mesh AimMesh(Transform transform
        ) { return null; }

    }

}

