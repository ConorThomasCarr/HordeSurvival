using AI.BaseNpc;
using UnityEngine;

public class AIMaster : MonoBehaviour
{
     private INpc _npc;
     
     private IFiniteStateMachine _finiteStateMachine;
   
     private IFiniteActionMachine _finiteActionMachine;
     
     private WeaponInventory _weaponInventory;
     
     private bool _initialized;

     private void Awake()
     {
          _npc = GetComponent<INpc>();
          _finiteStateMachine = gameObject.GetComponent<IFiniteStateMachine>();
          _finiteActionMachine = gameObject.GetComponent<IFiniteActionMachine>();
          _weaponInventory = GetComponent<WeaponInventory>();
     }

     private void OnDisable()
     {
          _npc?.Disable();
          
          _finiteStateMachine?.UninitializeFiniteStateMachine();
          _finiteActionMachine?.UninitializeFiniteActionMachine();
          
          _finiteStateMachine?.Disable();
          _finiteActionMachine?.Disable();
          _weaponInventory.enabled = false;
     }

     private void OnEnable()
     {
          _npc?.Enable();
          
          _finiteStateMachine?.Enable();
          _finiteActionMachine?.Enable();
          _weaponInventory.enabled = true;
          
          _finiteStateMachine?.InitializeFiniteStateMachine();
          _finiteActionMachine?.InitializeFiniteActionMachine();
          
          if (_initialized)
          {
               _npc?.InitializeFsmSystem?.Invoke();
               _npc?.InitializeWeapons.Invoke();
          }
     }

     private void Start()
     {
          _npc.InitializeConstruction?.Invoke();
          _npc.InitializeConfigs?.Invoke();
          _npc.InitializeFsmSystem?.Invoke();
          _npc?.InitializeWeapons.Invoke();
          
          _initialized = true;
     }

}
