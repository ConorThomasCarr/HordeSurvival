using System;
using System.Collections.Generic;

using AI.Config.Character;
using AI.Config.States;

using UnityEngine;
using UnityEngine.Events;

public class ZombieFiniteStateMachine : MonoBehaviour, IFiniteStateMachine
{
    public IStates CurrentState {get; set;}
    public List<IStates> ValidStates {get; set;}
    public Dictionary<FsmStatePhase, IStates> FsmStates {get; set;}
    
    public UnityAction InitializeFiniteStateMachine {get; set;} 
    public UnityAction UninitializeFiniteStateMachine {get; set;}
    
    public UnityAction <StateConfig, CharacterConfig> InitializeStateConfig {get; set;}
    
    private void OnInitializeFiniteStateMachine()
    {
        //Debug.Log("Zombie Finite State Machine On Initialize Finite State Machine: " + gameObject.name);
        
        foreach (var state in ValidStates)
        {
            FsmStates.Add(state.StatePhase, state);
        }
        
    }
    
    private void OnUninitializeFiniteStateMachine()
    {
        //Debug.Log("Zombie Finite State Machine On Uninitialize Finite State Machine: " + gameObject.name);
        
        foreach (var state in ValidStates)
        {
            state.UninitializeStateEvents();
            state.UninitializeState();
            
            FsmStates.Remove(state.StatePhase);
        }

       
    }

    public void Awake()
    {
        var noneStates = new NoneState();
        var exploreStates = new ExploreState();
        var searchStates = new SearchState();
        var chaseStates = new ChaseState();
        var combatStates = new CombatState();
        
        ValidStates = new List<IStates>();
        FsmStates = new Dictionary<FsmStatePhase, IStates>();

        ValidStates.Add(noneStates);
        ValidStates.Add(exploreStates);
        ValidStates.Add(searchStates);
        ValidStates.Add(chaseStates);
        ValidStates.Add(combatStates);
        
    }

    public void Enable()
    {
        //Debug.Log("Zombie Finite State Machine Enable: " + gameObject.name);
        
        InitializeFiniteStateMachine += OnInitializeFiniteStateMachine;
        UninitializeFiniteStateMachine += OnUninitializeFiniteStateMachine;
        InitializeStateConfig += OnInitializeStateConfig;

        enabled = true;

    }

    public void Disable()
    {
        //Debug.Log("Zombie Finite State Machine Disable: " + gameObject.name);
        
        InitializeFiniteStateMachine -= OnInitializeFiniteStateMachine;
        UninitializeFiniteStateMachine -= OnUninitializeFiniteStateMachine;
        InitializeStateConfig -= OnInitializeStateConfig;

        enabled = false;

    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    public void EnterState(IStates nextState)
    {
        if (nextState == null)
        {
            return;
        }
        
        CurrentState?.UninitializeStateEvents();
        CurrentState?.UninitializeState();
        CurrentState?.ExitState();

        CurrentState = nextState;
      
        CurrentState?.InitializeState();
        CurrentState?.InitializeStateEvents();
        CurrentState?.EnterState();
    }

    public void EnterStatePhase(FsmStatePhase stateType)
    {
        if (!FsmStates.TryGetValue(stateType, out var nextStates)) return;
        EnterState(nextStates);
    }
    
    private void OnInitializeStateConfig(StateConfig stateConfig, CharacterConfig characterConfig)  
    {
        foreach (var state in ValidStates)
        {
            state.OnInitializeStateConfigs(stateConfig, characterConfig);
        }
    }
    
}
