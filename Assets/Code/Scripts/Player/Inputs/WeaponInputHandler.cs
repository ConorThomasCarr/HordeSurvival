using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls.WeaponHandler
{
    public class WeaponInputHandler : MonoBehaviour
    {
        public static WeaponInputHandler Instance;
        
        private WeaponInventory _weaponInventory;
        
        public InputAction _shoot { get; set; }
       
        public InputAction _aim{ get; set; }
       
        public InputAction _reload{ get; set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            _shoot = InputSystem.actions.FindAction("Shoot");
            _aim = InputSystem.actions.FindAction("Aim");
            _reload = InputSystem.actions.FindAction("Reload");

            _weaponInventory = GameObject.FindWithTag("Player").transform.parent.GetComponent<WeaponInventory>();
        }


        private void Start()
        {
            _aim.canceled += AimReleased;
            _shoot.canceled += ShootReleased;
            _reload.canceled += ReloadReleased;
        }

        private void FixedUpdate()
        {
            if (_aim.inProgress)
            {
                _weaponInventory.HeldWeapon.AimIsInProgress?.Invoke();
            }
        }

        private void AimReleased(InputAction.CallbackContext context)
        {
            _weaponInventory.HeldWeapon.AimReleased?.Invoke();
        }
        
        private void ShootReleased(InputAction.CallbackContext context)
        {
            _weaponInventory.HeldWeapon.Shoot?.Invoke();
        } 
        
        private void ReloadReleased(InputAction.CallbackContext context)
        {
            _weaponInventory.HeldWeapon.Reload?.Invoke();
        }

       
    }
}
