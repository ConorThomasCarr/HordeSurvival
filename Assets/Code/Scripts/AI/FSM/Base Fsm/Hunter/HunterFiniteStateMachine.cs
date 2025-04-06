using System;
using System.Collections.Generic;

using AI.Config.Character;
using AI.Config.States;

using UnityEngine;
using UnityEngine.Events;

public class HunterFiniteStateMachine : MonoBehaviour, IFiniteStateMachine
{
    public IStates CurrentState {get; set;}
    public List<IStates> ValidStates {get; set;}
    public Dictionary<FsmStatePhase, IStates> FsmStates {get; set;}
    
    public UnityAction InitializeFiniteStateMachine {get; set;} 
    public UnityAction UninitializeFiniteStateMachine {get; set;}
    
    public UnityAction <StateConfig, CharacterConfig> InitializeStateConfig {get; set;}
    
    private void OnInitializeFiniteStateMachine()
    {
        foreach (var state in ValidStates)
        {
            FsmStates.Add(state.StatePhase, state);
        }
        
    }
    
    private void OnUninitializeFiniteStateMachine()
    {
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
        var guardStates = new GuardState();
        var roamStates = new RoamState();
        
        ValidStates = new List<IStates>();
        FsmStates = new Dictionary<FsmStatePhase, IStates>();

        ValidStates.Add(noneStates);
        ValidStates.Add(guardStates);
        ValidStates.Add(roamStates);
    }

    public void Enable()
    {
        InitializeFiniteStateMachine += OnInitializeFiniteStateMachine;
        UninitializeFiniteStateMachine += OnUninitializeFiniteStateMachine;
        InitializeStateConfig += OnInitializeStateConfig;

        enabled = true;
    }

    public void Disable()
    {
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
