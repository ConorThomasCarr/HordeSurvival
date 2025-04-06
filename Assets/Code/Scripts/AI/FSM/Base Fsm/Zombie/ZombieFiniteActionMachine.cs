using System.Collections.Generic;
using AI.Config.Actions;
using AI.Config.Character;
using UnityEngine;
using UnityEngine.Events;

public class ZombieFiniteActionMachine : MonoBehaviour, IFiniteActionMachine
{
    public IActions CurrentAction {get; set;}
    
    public List<IActions> ValidAction {get; set;}
    
    public Dictionary<FsmActionPhase, IActions> FsmActions {get; set;}
    
    public UnityAction InitializeFiniteActionMachine {get; set;}
    public UnityAction UninitializeFiniteActionMachine {get; set;}
    
    public UnityAction<ActionConfig, CharacterConfig> InitializeActionConfig { get; set; }

    private void OnInitializeFiniteActionMachine()
    {
        ////Debug.Log("Zombie Finite Action Machine On Initialize Finite Action Machine: " + gameObject.name);
        
        foreach (var action in ValidAction)
        {
            FsmActions.Add(action.ActionPhase, action);
        }
    }
    
    private void OnUninitializeFiniteActionMachine()
    {
        ////Debug.Log("Zombie Finite Action Machine On Uninitialize Finite Action Machine: " + gameObject.name);
        
        foreach (var action in ValidAction)
        {
            action.UninitializeAction();
            action.UninitializeActionEvents();
            
            FsmActions.Remove(action.ActionPhase);
        }
        
        //Debug.Log(FsmActions.Count);
    }
    
    
    public void Awake()
    {
        var noneActions = new NoneAction();
        var idleActions = new ExploreAction();
        var searchActions = new SearchAction();
        var chaseActions = new ChaseAction();
        var combatActions = new CombatAction();
        
        ValidAction = new List<IActions>();
        FsmActions = new Dictionary<FsmActionPhase, IActions>();

        ValidAction.Add(noneActions);
        ValidAction.Add(idleActions);
        ValidAction.Add(searchActions);
        ValidAction.Add(chaseActions);
        ValidAction.Add(combatActions);
    }

    public void Enable() 
    {
        ////Debug.Log("Zombie Finite Action Machine Enable: " + gameObject.name);
        
        InitializeFiniteActionMachine += OnInitializeFiniteActionMachine;
        UninitializeFiniteActionMachine += OnUninitializeFiniteActionMachine;
        InitializeActionConfig += OnInitializeActionConfig;
        
        enabled = true;
    }

    public void Disable()
    {
        ////Debug.Log("Zombie Finite Action Machine Disable: " + gameObject.name);
        
        InitializeFiniteActionMachine -= OnInitializeFiniteActionMachine;
        UninitializeFiniteActionMachine -= OnUninitializeFiniteActionMachine;
        InitializeActionConfig -= OnInitializeActionConfig;

        enabled = false;
    }
    
    public void Update()  
    {
        CurrentAction?.UpdateAction();
    }

    public void EnterAction(IActions nextAction) 
    {
        if (nextAction == null)
        {
            return;
        }
        
        CurrentAction?.UninitializeActionEvents();
        CurrentAction?.UninitializeAction();
        CurrentAction?.ExitAction();

        CurrentAction = nextAction;
      
        CurrentAction?.InitializeAction();
        CurrentAction?.InitializeActionEvents();
        CurrentAction?.EnterAction();
    }

    public void EnterActionPhase(FsmActionPhase actionType)  
    {
        if (!FsmActions.TryGetValue(actionType, out var nextActions)) return;
        EnterAction(nextActions);
    }

    private void OnInitializeActionConfig(ActionConfig actionConfig, CharacterConfig characterConfig)  
    {
        foreach (var actions in ValidAction)
        {
            actions.OnInitializeActionConfigs(actionConfig, characterConfig);
        }
    }

}
