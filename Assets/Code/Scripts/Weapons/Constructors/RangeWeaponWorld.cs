using UnityEngine;
using Weapon.BaseWeapon;
using Weapon.BaseWeapon.Gun.Pistol;
using Weapon.BaseWeapon.Gun.Shotgun;
using Weapon.BaseWeapon.RangeWeapons;

namespace Weapon.RangeWeaponConstructors
{
    public class RangeWeaponWorld <T> : RangeWeapon where T : IRangeWeapon, new()
    {
        public RangeWeaponWorld(GunTypes type, IWeapon weapon)
        {
            switch (type)
            {
                case GunTypes.Magnum:

                    var magnum = new T();
                    var rangeWeaponMagnum = magnum.CreateMagnum();
                    var localMagnumWeapon = (Pistol)weapon;
                
                    localMagnumWeapon.InitializeGun?.Invoke(rangeWeaponMagnum);
                
                    break;
            
                case GunTypes.DoubleBarrel:

                    var doubleBarrel = new T();
                    var rangeWeaponDoubleBarrel = doubleBarrel.CreateDoubleBarrel();
                    var localDoubleBarrelWeapon = (Shotgun)weapon;
                
                    localDoubleBarrelWeapon.InitializeGun?.Invoke(rangeWeaponDoubleBarrel);
                
                    break;
            
            }
        }
    }
}

