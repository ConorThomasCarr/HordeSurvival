using AI.Config.Character;
using AI.Config.States;
using UnityEngine;

public interface IStates
{

    FsmStatePhase StatePhase { get; set; }

    StateConfig StateConfig { get; set; }

    CharacterConfig CharacterConfig { get; set; }

    bool EnteredState { get; set; }

    bool EnterTaskStateCondition { get; set; }

    bool ExitTaskStateCondition { get; set; }

    void OnInitializeStateConfigs(StateConfig stateConfig, CharacterConfig characterConfig);

    void InitializeState();

    void UninitializeState();

    void InitializeStateEvents();

    void UninitializeStateEvents();

    void EnterState();

    void UpdateState();

    void ExitState();

    void OnContinueState(OnContinueState onContinueState);

    void OnDiscontinueState(OnDiscontinueState onDiscontinueState);

    void OnEnterTaskStateCondition(OnEnterTaskStateCondition onEnterTaskStateCondition);

    void OnExitTaskStateCondition(OnExitTaskStateCondition onExitTaskStateCondition);
}

public abstract class BaseState : IStates
{
    public FsmStatePhase StatePhase { get; set; }

    public StateConfig StateConfig { get; set; }

    public CharacterConfig CharacterConfig { get; set; }

    public bool EnteredState { get; set; }

    public bool EnterTaskStateCondition { get; set; }

    public bool ExitTaskStateCondition { get; set; }

    public virtual void OnInitializeStateConfigs(StateConfig stateConfig, CharacterConfig characterConfig)
    {
        StateConfig = stateConfig;
        CharacterConfig = characterConfig;
    }

    public virtual void InitializeState()
    {
    }


    public virtual void UninitializeState()
    {
    }

    public virtual void InitializeStateEvents()
    {
    }

    public virtual void UninitializeStateEvents()
    {
    }

    public virtual void EnterState()
    {
    }

    public virtual void UpdateState()
    {
    }

    public virtual void ExitState()
    {
    }

    public virtual void OnContinueState(OnContinueState onContinueState)
    {
    }

    public virtual void OnDiscontinueState(OnDiscontinueState onDiscontinueState)
    {
    }

    public virtual void OnEnterTaskStateCondition
        (OnEnterTaskStateCondition onEnterTaskStateCondition)
    {
    }

    public virtual void OnExitTaskStateCondition
        (OnExitTaskStateCondition onExitTaskStateCondition)
    {
    }
}
