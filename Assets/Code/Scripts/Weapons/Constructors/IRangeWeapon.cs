using UnityEngine;
using Weapon.BaseGun;

namespace Weapon.BaseWeapon.RangeWeapons
{
    public interface IRangeWeapon
    {
        IGun CreateMagnum();

        IGun CreateDoubleBarrel();
    }
}


