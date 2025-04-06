using UnityEngine;

public class NoneState : BaseState
{
    public NoneState()
    {
        StatePhase = FsmStatePhase.None;
    }

    public override void InitializeState()
    {
        
    }

    public override void UninitializeState()
    {
        
    }

    public override void InitializeStateEvents()
    {
        EventManager.AddListener<OnContinueState>(OnContinueState);
        EventManager.AddListener<OnDiscontinueState>(OnDiscontinueState);
        EventManager.AddListener<OnEnterTaskStateCondition>(OnEnterTaskStateCondition);
        EventManager.AddListener<OnExitTaskStateCondition>(OnExitTaskStateCondition);
    }

    public override void UninitializeStateEvents()
    {
        EventManager.RemoveListener<OnContinueState>(OnContinueState);
        EventManager.RemoveListener<OnDiscontinueState>(OnDiscontinueState);
        EventManager.RemoveListener<OnEnterTaskStateCondition>(OnEnterTaskStateCondition);
        EventManager.RemoveListener<OnExitTaskStateCondition>(OnExitTaskStateCondition);
    }

    public override void EnterState()
    {
        if (!EnteredState)
        {
            EnteredState = (CharacterConfig.NpcGeneralConfig.Name != "");

            var evtOnEnter = FsmStateEvents.OnEnterState;
            evtOnEnter.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtOnEnter.Enter = true;
            EventManager.Broadcast(evtOnEnter);
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.StateSender = "";
            evtOnChangeTask.state = FsmStatePhase.None;
            EventManager.Broadcast(evtOnChangeTask);
        }
    }

    public override void UpdateState()
    {
        if (EnteredState && EnterTaskStateCondition)
        {
            var evtDiscontinueAction = FsmActionEvents.OnDiscontinueAction;
            evtDiscontinueAction.Sender = CharacterConfig.NpcGeneralConfig.Name;
            EventManager.Broadcast(evtDiscontinueAction);
        }

        if (ExitTaskStateCondition)
        {
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.StateSender = CharacterConfig.NpcGeneralConfig.Name;
            evtOnChangeTask.state = StateConfig.StateList[0];
            EventManager.Broadcast(evtOnChangeTask);
        }

    }

    public override void ExitState()
    {
        EnteredState = false;
    }

    public override void OnContinueState(OnContinueState onContinueState)
    {
        if (onContinueState.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            
        }
    }

    public override void OnDiscontinueState(OnDiscontinueState onDiscontinueState)
    {
        if (onDiscontinueState.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            var evtExit = FsmStateEvents.OnExitState;
            evtExit.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtExit.Exited = true;
            EventManager.Broadcast(evtExit);
        }
    }

    public override void OnEnterTaskStateCondition(OnEnterTaskStateCondition onEnterTaskStateCondition)
    {
        if (onEnterTaskStateCondition.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            EnterTaskStateCondition = onEnterTaskStateCondition.Condition;
            ExitTaskStateCondition = false;
        }
    }

    public override void OnExitTaskStateCondition(OnExitTaskStateCondition onExitTaskStateCondition)
    {
        if (onExitTaskStateCondition.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            ExitTaskStateCondition = onExitTaskStateCondition.Condition;
            EnterTaskStateCondition = false;
        }
    }
}

