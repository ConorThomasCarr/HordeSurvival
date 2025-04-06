using UnityEngine;
using Weapon.BaseGun;
using Weapon.BaseGun.BaseDoubleBarrelShotgun.DoubleBarrelShotgun;
using Weapon.BaseGun.BaseMagnum.Magnum;

namespace Weapon.BaseWeapon.RangeWeapons
{
    public class RangeWeapon : IRangeWeapon
    {
        public IGun CreateMagnum() => new Magnum();

        public IGun CreateDoubleBarrel() => new DoubleBarrelShotgun();
    }
}


