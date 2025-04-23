using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Weapon.BaseGun;
using Weapon.BaseWeapon.Gun.Pistol.Data;
using Weapon.BaseWeapon.RangeWeapons;
using Weapon.RangeWeaponConstructors;

namespace Weapon.BaseWeapon.Gun.Pistol
{
    public class Pistol : Gun
    {
        private MeshFilter _meshFilter;

        private bool _isAiming;

        private bool _isReloading;

        private bool _actionAllowed;

        public Image reloadImage;

        public TMP_Text ammoText;

        public float reloadTime;

        [SerializeField]
        private PistolData pistolData;
        
        public void Awake()
        {
            try
            {
                _meshFilter = transform.parent.Find("Gun Aim Angle Mesh").GetComponent<MeshFilter>();

            }
            catch
            {
                // ignored
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
                WeaponRangeMinAngle = pistolData.weaponRangeMinAngle,
                WeaponRangeMaxAngle = pistolData.weaponRangeMaxAngle,
            
                WeaponSpreadMaxAngle = pistolData.weaponSpreadMaxAngle,
                WeaponSpreadMinAngle = pistolData.weaponSpreadMinAngle,
               
                magazineCapacity = pistolData.magazineCapacity,
                maxAmmoCapacity = pistolData.maxAmmoCapacity,

                reloadCooldown = pistolData.reloadCooldown,
                
                spreadSpeed = pistolData.spreadSpeed,
                aimSpeed = pistolData.aimSpeed,
            };

           WeaponConfig = new WeaponConfig(baseData, rangeData);
            
           WeaponInterface?.InitializeConfig(WeaponConfig);
           
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

            if (ammoText != null)
            {
                ammoText.text = WeaponInterface.magazineSize + "\r\n" + "---" + " \r\n" +
                                WeaponInterface.maxAmmo;
            }
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
            if (reloadTime == 0)
            {
                WeaponInterface.Reload();
            }
        }

        private void Start()
        {
            if (ammoText != null)
            {
                ammoText.text = WeaponInterface.magazineSize + "\r\n" + "---" + " \r\n" +
                                WeaponInterface.maxAmmo;
            }
        }

        public void Update()
        {
            if (_actionAllowed)
            {
                if (_meshFilter != null)
                {
                    _meshFilter.mesh = WeaponInterface.AimMesh(transform);

                }
                
                if (_isAiming == false)
                {
                    WeaponInterface.HasNoAim();
                }

                if (_isAiming)
                {
                    WeaponInterface.HasAim();
                }

                if (WeaponInterface.isReloading)
                {

                    if (reloadImage != null)
                    {
                        reloadImage.fillAmount = reloadTime / WeaponConfig.CoreConfig.reloadCooldown;
                    }

                    reloadTime += Time.deltaTime;

                    if (reloadTime >= WeaponConfig.CoreConfig.reloadCooldown)
                    {
                        WeaponInterface.Reload();

                        reloadTime = 0;

                        if (reloadImage != null && ammoText != null)
                        {
                            reloadImage.fillAmount = 0;

                            ammoText.text = WeaponInterface.magazineSize + "\r\n" + "---" + " \r\n" +
                                            WeaponInterface.maxAmmo;
                        }
                    }
                }
            }
            
            
        }
    }
}

namespace Weapon.BaseWeapon.Gun.Pistol.Data
{
    [Serializable]
    public struct PistolData
    {
        
        public float weaponRangeMaxAngle {get => _weaponRangeMaxAngle; set => weaponRangeMaxAngle = value;}
        public float weaponRangeMinAngle {get => _weaponRangeMinAngle; set => weaponRangeMinAngle = value;}
        
        public float weaponSpreadMaxAngle {get => _weaponSpreadMaxAngle; set => weaponSpreadMaxAngle = value;}
        public float weaponSpreadMinAngle {get => _weaponSpreadMinAngle; set => weaponSpreadMinAngle  = value;}
        
        public float reloadCooldown {get => _reloadCooldown; set => reloadCooldown = value;}
        public float swapWeaponCooldown {get => _swapWeaponCooldown; set => swapWeaponCooldown = value;}
        
        public int magazineCapacity {get => _magazineCapacity; set => magazineCapacity = value;}
        public int maxAmmoCapacity {get => _maxAmmoCapacity; set => maxAmmoCapacity = value;}
        
        public float spreadSpeed {get => _spreadSpeed ; set =>spreadSpeed  = value;}
        public float aimSpeed {get => _aimSpeed; set => aimSpeed = value;}
       
        [Header ("Weapon Aim Angle")]
        [SerializeField] private float _weaponRangeMaxAngle;
        [SerializeField] private float _weaponRangeMinAngle;
        
        [Header ("Weapon Spread Angle")]
        [SerializeField] private float _weaponSpreadMaxAngle;
        [SerializeField] private float _weaponSpreadMinAngle;
        
        [Header ("Weapon Reload")]
        [SerializeField] private float _reloadCooldown;
        [SerializeField] private float _swapWeaponCooldown;
        
        [Header ("Weapon Ammo")]
        [SerializeField] private int _magazineCapacity;
        [SerializeField] private int _maxAmmoCapacity;
        
        [Header ("Weapon Aim Speed")]
        [SerializeField] private float _spreadSpeed;
        [SerializeField] private float _aimSpeed;
    }
}

