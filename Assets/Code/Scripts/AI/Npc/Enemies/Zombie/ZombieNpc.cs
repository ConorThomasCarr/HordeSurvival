using System;
using System.Collections.Generic;
using AI.BaseCharacters;
using AI.BaseCharacters.Undead;
using AI.BaseCharacters.Undead.BaseZombie;
using AI.BaseNpc.Undead;
using AI.Config.Actions;
using AI.Config.Character;
using AI.Config.Navmesh;
using AI.Config.NpcGeneral;
using AI.Config.States;
using AI.UndeadConstructors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


namespace AIUndeadEnemy
{
    public class ZombieNpc : MonoBehaviour, IUndeadNpc
    {
        private BaseZombie _zombieInterface;
        
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

        private SensorMaster _visionSensorMaster;
        
        private WeaponInventory _weaponInventory;
        
        
        private NavMeshAgent _agent;
        
        public void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            
            _finiteActionMachine = GetComponent<IFiniteActionMachine>();
            _finiteStateMachine = GetComponent<IFiniteStateMachine>();
            
            _visionSensorMaster = transform.Find("Vision Sensor System").GetComponent<SensorMaster>();
            
            _weaponInventory = GetComponent<WeaponInventory>();
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
            InitializeWeapons -= OnInitializeWeapons;
            
            StatePhaseChanged -= OnStatePhaseChanged;
            ActionPhaseChanged -= OnActionPhaseChanged;

            _visionSensorMaster.enabled = false;
            
            enabled = false;
        }

        private void OnInitializeConstruction()
        {
            var undead = new UndeadWorld<Undead>(UndeadType.Zombie, this);
        }
        
        private void OnInitializeNpc(ICharacters characters)
        {
            //Debug.Log("On Initialize Npc: " + gameObject.name);
                
            _zombieInterface = (BaseZombie)characters; 
        }

        private void OnInitializeConfigs()
        {
            List<FsmActionPhase> actions = new List<FsmActionPhase>();
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            
            actions.Add(FsmActionPhase.Explore);
            actions.Add(FsmActionPhase.Search);
            actions.Add( FsmActionPhase.Chase);
            actions.Add(FsmActionPhase.Combat);
            
            var actionConfig = new ActionConfig(actions);
            
            List<FsmStatePhase> states = new List<FsmStatePhase>();
            if (states == null) throw new ArgumentNullException(nameof(states));
            
            states.Add(FsmStatePhase.Explore);
            states.Add(FsmStatePhase.Search);
            states.Add(FsmStatePhase.Chase);
            states.Add(FsmStatePhase.Combat);

            var stateConfig = new StateConfig(states);
            
            var npcGeneralConfig = new NpcGeneralConfig(gameObject.name);
            var navmeshConfig = new NavmeshConfig(_agent);
            var characterConfig = new CharacterConfig(navmeshConfig , npcGeneralConfig);
            
            _finiteActionMachine.InitializeActionConfig(actionConfig, characterConfig);
            _finiteStateMachine.InitializeStateConfig(stateConfig, characterConfig);
            
            _zombieInterface.InitializeConfig(characterConfig);
            _zombieInterface.InitializeEvents();
            _zombieInterface.InitializeNpc(this);
        }

        private void OnInitializeFsmSystem()
        {
            _finiteStateMachine.EnterStatePhase(FsmStatePhase.None);
            _finiteActionMachine.EnterActionPhase(FsmActionPhase.None);
        }
        
        private void OnInitializeWeapons()
        {
            _weaponInventory?.InitializeWeapons();
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

        private void Update()
        {
            
        }
    }
}


