using System.Collections.Generic;

using AI.BaseCharacters;
using AI.BaseCharacters.Humans;
using AI.BaseCharacters.Humans.BaseHunter;

using AI.HumanConstructors;   

using AI.BaseNpc.Human;
using AI.Config.Actions;
using AI.Config.Character;
using AI.Config.Navmesh;
using AI.Config.NpcGeneral;
using AI.Config.States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class HunterNpc : MonoBehaviour, IHumanNpc
{ 
    private BaseHunter _hunterInterface;
    public UnityAction InitializeConstruction { get; set; }
    public UnityAction <ICharacters> InitializeNpc { get; set; }
    public UnityAction InitializeConfigs { get; set; }
    public UnityAction InitializeFsmSystem{ get; set; }
    
    public UnityAction InitializeWeapons{ get; set; }
    public UnityAction<FsmStatePhase> StatePhaseChanged { get; set; }
    public  UnityAction<FsmActionPhase> ActionPhaseChanged { get; set; }
    private FsmStatePhase StatePhase {get; set;}
    private FsmActionPhase ActionPhase {get; set;}
    
    private IFiniteStateMachine _finiteStateMachine;
      
    private IFiniteActionMachine _finiteActionMachine;

    private WeaponInventory _weaponInventory;
    
    private SensorMaster _visionSensorMaster;
    
    private NavMeshAgent _agent;
    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        _agent.updateRotation = false;
            
        _finiteActionMachine = GetComponent<IFiniteActionMachine>();
        _finiteStateMachine = GetComponent<IFiniteStateMachine>();
        _weaponInventory = GetComponent<WeaponInventory>();
        
        _visionSensorMaster = transform.GetChild(0).Find("Vision Sensor System").GetComponent<SensorMaster>();
    }

    
    public void Enable()
    {
        InitializeConstruction += OnInitializeConstruction;
        InitializeNpc += OnInitializeNpc;
           
        InitializeConfigs += OnInitializeConfigs;
        InitializeFsmSystem += OnInitializeFsmSystem;
        InitializeWeapons += OnInitializeWeapons;
            
        StatePhaseChanged += OnStatePhaseChanged;
        ActionPhaseChanged += OnActionPhaseChanged;
        
        _visionSensorMaster.enabled = true;
        
        enabled = true;
    }

    public void Disable()
    {
        InitializeConstruction -= OnInitializeConstruction;
        InitializeNpc -= OnInitializeNpc;
           
        InitializeConfigs -= OnInitializeConfigs;
        InitializeFsmSystem -= OnInitializeFsmSystem;
        InitializeWeapons += OnInitializeWeapons;
 
            
        StatePhaseChanged -= OnStatePhaseChanged;
        ActionPhaseChanged -= OnActionPhaseChanged;

        _visionSensorMaster.enabled = false;
        
        enabled = false;
    }

    private void OnInitializeConstruction()
    {
        var human = new HumanWorld<Human>(HumanTypes.Hunter, this);
    }
    

    private void OnInitializeNpc(ICharacters characters)
    {
        _hunterInterface = (BaseHunter)characters;
    }
    
    
    private void OnInitializeConfigs()
    {
        List<FsmActionPhase> actions = new List<FsmActionPhase>();
     
        actions.Add(FsmActionPhase.Guard);
        actions.Add(FsmActionPhase.Roam);
        
        var actionConfig = new ActionConfig(actions);
 
        List<FsmStatePhase> states = new List<FsmStatePhase>();
     
        states.Add(FsmStatePhase.Guard);
        states.Add(FsmStatePhase.Roam);
        
        var stateConfig = new StateConfig(states);
        
        var npcGeneralConfig = new NpcGeneralConfig(gameObject.name);
        var navmeshConfig = new NavmeshConfig(_agent);
     
        var characterConfig = new CharacterConfig(navmeshConfig , npcGeneralConfig);
     
        _finiteActionMachine.InitializeActionConfig(actionConfig, characterConfig);
        _finiteStateMachine.InitializeStateConfig(stateConfig, characterConfig);
    
        _hunterInterface.InitializeConfig(characterConfig);
        _hunterInterface.InitializeEvents();
        _hunterInterface.InitializeNpc(this);
        
    }
        
    private  void OnInitializeFsmSystem()
    {
        _finiteStateMachine.EnterStatePhase(FsmStatePhase.None);
        _finiteActionMachine.EnterActionPhase(FsmActionPhase.None);
    }
    
    private void OnInitializeWeapons()
    {
        _weaponInventory?.InitializeWeapons();
    }
    
    public void Start()
    {
     
    }

    private void Update()
    {
        
    }
    
    private void OnStatePhaseChanged(FsmStatePhase phase)
    { 
        StatePhase = phase;
        _finiteStateMachine.EnterStatePhase(StatePhase);
    }
    
    private void OnActionPhaseChanged(FsmActionPhase action)
    {
        ActionPhase = action;
        _finiteActionMachine.EnterActionPhase(ActionPhase);
    } 
}
