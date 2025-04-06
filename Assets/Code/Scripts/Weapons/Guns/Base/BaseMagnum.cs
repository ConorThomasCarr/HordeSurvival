using UnityEngine;

namespace Weapon.BaseGun.BaseMagnum
{
    public abstract class BaseMagnum : IGun
    {
        public float WeaponSpread {get; set;}
    
        public float AimAngle {get; set;}
    
        public WeaponConfig weaponConfig {get; set;}

        public virtual void InitializeEvents() { }

        public virtual void InitializeConfig(WeaponConfig weaponData) { weaponConfig = weaponData; }
        
        public virtual void Shoot() { }
    
        public virtual void Aim() { }
        
        public virtual void Reload(){ }
    
        public virtual void HasAim(){ }
    
        public virtual void HasNoAim() { }

        public virtual bool CanShoot() { return false; }
    
        public virtual bool CanAim() {return false;}
    
        public virtual  bool IsAiming() { return false; }

        public virtual Mesh AimMesh(Transform transform
        ) { return null; }

    }
}


