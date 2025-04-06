using AI.Config.Actions;
using AI.Config.Character;
using UnityEngine;

public interface IActions
{
    FsmActionPhase ActionPhase { get; set; }
    
    ActionConfig ActionConfig { get; set; }
    
    CharacterConfig CharacterConfig { get; set; }
    
    bool EnteredAction { get; set; } 
    
    bool EnterTaskActionCondition { get; set; }
    
   bool ExitTaskActionCondition { get; set; }
   
   void OnInitializeActionConfigs(ActionConfig actionConfigModule, CharacterConfig characterConfig);
    
    void InitializeAction();

    void UninitializeAction();

    void InitializeActionEvents();

    void UninitializeActionEvents();

    void EnterAction();

    void UpdateAction();

    void ExitAction();

    void ContinueAction(OnContinueAction onContinueAction);

    void OnDiscontinueAction(OnDiscontinueAction onDiscontinueAction);

    void OnEnterTaskActionCondition(OnEnterTaskActionCondition onEnterTaskActionCondition);

    void OnExitTaskActionCondition(OnExitTaskActionCondition onExitTaskActionCondition);
}



public abstract class BaseActions : IActions
{
    public FsmActionPhase ActionPhase { get; set; }
    
    public ActionConfig ActionConfig { get; set; }

    public CharacterConfig CharacterConfig { get; set; }
    
    public bool EnteredAction { get; set; } 
    
    public bool EnterTaskActionCondition { get; set; }

    public bool ExitTaskActionCondition { get; set; }

    public virtual void OnInitializeActionConfigs
        (ActionConfig actionConfigModule, CharacterConfig characterConfig) 
    {
        ActionConfig = actionConfigModule;
        CharacterConfig = characterConfig;
    }
    
    public virtual void InitializeAction() { }

    public virtual void UninitializeAction() { }
    

    public virtual void InitializeActionEvents() { }

    public virtual void UninitializeActionEvents() { }

    public virtual void EnterAction() { }

    public virtual void UpdateAction() { }

    public virtual void ExitAction() { }
    
    public virtual void ContinueAction(OnContinueAction onContinueAction) { }
        
    public virtual void OnDiscontinueAction(OnDiscontinueAction onDiscontinueAction) { }
    
    public virtual void OnEnterTaskActionCondition
        (OnEnterTaskActionCondition onEnterTaskActionCondition) { }
    
    public virtual void OnExitTaskActionCondition
        (OnExitTaskActionCondition onExitTaskActionCondition) { }
    
}
