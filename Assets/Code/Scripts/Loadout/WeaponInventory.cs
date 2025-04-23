using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using Weapon.BaseWeapon;

public class WeaponInventory : MonoBehaviour
{
    public UnityAction InitializeWeapons {get; set;}
   
    public UnityAction SwapWeapons {get; set;}
    
    private IWeapon _primaryWeapon;
    private IWeapon _secondaryWeapon;
    
    public IWeapon HeldWeapon {get; set;}
    
    private void Awake()
    {
        _primaryWeapon = transform.GetChild(0).Find("Weapons").GetChild(0).GetComponent<IWeapon>();
        _secondaryWeapon = transform.GetChild(0).Find("Weapons").GetChild(1).GetComponent<IWeapon>();
    }

    private void Start()
    {
        HeldWeapon = _primaryWeapon;
        HeldWeapon?.InitializeWeaponAction();
       
        SwapWeapons += OnSwapWeapons;
    }

    private void Update()
    {
       
    }


    private void OnEnable()
    {
        _primaryWeapon?.Enable();
        _secondaryWeapon?.Enable();

        InitializeWeapons += OnInitializeWeapons;
    }
    
    private void OnDisable()
    {
        _primaryWeapon?.Disable();
        _secondaryWeapon?.Disable();
        
        InitializeWeapons -= OnInitializeWeapons;
    }

    private void OnInitializeWeapons()
    {
        _primaryWeapon?.InitializeConstruction();
        _secondaryWeapon?.InitializeConstruction();
        
        _primaryWeapon?.InitializeConfigs();
        _secondaryWeapon?.InitializeConfigs();
    }
    
    private void OnSwapWeapons()
    {
        if (HeldWeapon == _secondaryWeapon)
        {
            HeldWeapon?.UninitializeWeaponAction();
            
            HeldWeapon = _primaryWeapon;
            
            HeldWeapon?.InitializeWeaponAction();
            
            return;
        }
        
        if (HeldWeapon == _primaryWeapon)
        {
            HeldWeapon?.UninitializeWeaponAction();
            
            HeldWeapon = _secondaryWeapon;
            
            HeldWeapon?.InitializeWeaponAction();
        }
        
    }
    
}
