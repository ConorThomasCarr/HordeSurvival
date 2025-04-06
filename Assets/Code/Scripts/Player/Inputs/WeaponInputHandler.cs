using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls.WeaponHandler
{
    public class WeaponInputHandler : MonoBehaviour
    {
        private WeaponInventory _weaponInventory;
        
        private InputAction _shoot;
        private InputAction _aim;
        private InputAction _reload;
        
        private void Awake()
        {
            _shoot = InputSystem.actions.FindAction("Shoot");
            _aim = InputSystem.actions.FindAction("Aim");
            _reload = InputSystem.actions.FindAction("Reload");

            _weaponInventory = FindFirstObjectByType<WeaponInventory>();
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
