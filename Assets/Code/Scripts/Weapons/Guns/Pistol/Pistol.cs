using System;
using UnityEngine;
using Weapon.BaseGun;
using Weapon.BaseWeapon.RangeWeapons;
using Weapon.RangeWeaponConstructors;

namespace Weapon.BaseWeapon.Gun.Pistol
{
    public class Pistol : Gun
    {
        private MeshFilter _meshFilter;

        private bool _isAiming;

        private bool _actionAllowed;
        public void Awake()
        {
            try
            {
                _meshFilter = transform.parent.GetChild(2).GetComponent<MeshFilter>();
            }
            catch
            {

            }
        }

        public override void Enable()
        {
            InitializeConstruction += OnInitializeConstruction;
            InitializeGun += OnInitializeGun;

            InitializeConfigs += OnInitializeConfigs;
            InitializeWeaponAction += OnInitializeWeaponAction;
            UninitializeWeaponAction += OnUninitializeWeaponAction;

            enabled = true;
        }

        public override void Disable()
        {
            InitializeConstruction -= OnInitializeConstruction;
            InitializeGun -= OnInitializeGun;

            InitializeConfigs -= OnInitializeConfigs;

            InitializeWeaponAction -= OnInitializeWeaponAction;
            UninitializeWeaponAction -= OnUninitializeWeaponAction;

            enabled = false;
        }

        private void OnInitializeConstruction()
        {
            var rangeWeapon = new RangeWeaponWorld<RangeWeapon>
                (GunTypes.Magnum, this);
        }

        private void OnInitializeGun(IGun gun)
        {
            WeaponInterface = gun;
            WeaponInterface?.InitializeEvents();
        }

        private void OnInitializeConfigs()
        {
            var baseData = new WeaponGeneralConfig
                { Parent = transform.root.gameObject, MuzzleOne = transform.GetChild(0), };

            var rangeData = new WeaponCoreConfig
            {
                WeaponRangeMinAngle = 5,
                WeaponRangeMaxAngle = 40,
                WeaponSpreadMaxAngle = 20,
                WeaponSpreadMinAngle = 5,
                magazineSize = 5,
                magazineCapacity = 5,
                maxAmmoCapacity = 40,
            };

            WeaponData = new WeaponConfig(baseData, rangeData);

            WeaponInterface?.InitializeConfig(WeaponData);
        }

        private void OnInitializeWeaponAction()
        {
            Shoot += OnShoot;
            Reload += OnReload;
            AimIsInProgress += OnAim;
            AimReleased += OnAimReleased;
            
            _actionAllowed = true;

        }

        private void OnUninitializeWeaponAction()
        {
            Shoot -= OnShoot;
            Reload -= OnReload;
            AimIsInProgress -= OnAim;
            AimReleased -= OnAimReleased;
            
            _isAiming = false;
            _actionAllowed = false;
        }

        private void OnShoot()
        {
            WeaponInterface.Shoot();
        }

        private void OnAim()
        {
            if (WeaponInterface.CanAim())
            {
                _isAiming = true;
            }
        }


        private void OnAimReleased()
        {
            _isAiming = false;
        }

        private void OnReload()
        {
            WeaponInterface.Reload();
        }

        public void Update()
        {
            if (_actionAllowed)
            {
                if (_isAiming == false)
                {
                    if (_meshFilter != null)
                    {
                        _meshFilter.mesh = WeaponInterface.AimMesh(transform);
                    }

                    WeaponInterface.HasNoAim();
                }

                if (_isAiming)
                {
                    if (_meshFilter != null)
                    {
                        _meshFilter.mesh = WeaponInterface.AimMesh(transform);
                    }


                    WeaponInterface.HasAim();
                }
            }
        }
    }
}


