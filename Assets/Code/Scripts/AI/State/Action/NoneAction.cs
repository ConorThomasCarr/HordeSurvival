using UnityEngine;

public class NoneAction : BaseActions
{
    public NoneAction()
    {
        ActionPhase = FsmActionPhase.None;
    }
    
    public override void InitializeAction()
    {
        
    }

    public override void UninitializeAction()
    {
        
    }

    public override void InitializeActionEvents()
    {
        EventManager.AddListener<OnContinueAction>(ContinueAction);
        EventManager.AddListener<OnDiscontinueAction>(OnDiscontinueAction);
        EventManager.AddListener<OnEnterTaskActionCondition>(OnEnterTaskActionCondition);
        EventManager.AddListener<OnExitTaskActionCondition>(OnExitTaskActionCondition);
    }

    public override void UninitializeActionEvents()
    {
        EventManager.RemoveListener<OnContinueAction>(ContinueAction);
        EventManager.RemoveListener<OnDiscontinueAction>(OnDiscontinueAction);
        EventManager.RemoveListener<OnEnterTaskActionCondition>(OnEnterTaskActionCondition);
        EventManager.RemoveListener<OnExitTaskActionCondition>(OnExitTaskActionCondition);
    }

    public override void EnterAction()
    {
        if (!EnteredAction)
        {
            EnteredAction = (CharacterConfig.NpcGeneralConfig.Name != "");
            
            var evtActionEnter = FsmActionEvents.OnEnterAction;
            evtActionEnter.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtActionEnter.Enter = true;
            EventManager.Broadcast(evtActionEnter);
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.ActionSender = "";
            evtOnChangeTask.action = FsmActionPhase.None;
            EventManager.Broadcast(evtOnChangeTask);
        }
    }

    public override void UpdateAction()
    {
        if (EnteredAction && EnterTaskActionCondition)
        {
        }

        if (ExitTaskActionCondition)
        {
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.ActionSender = CharacterConfig.NpcGeneralConfig.Name;
            evtOnChangeTask.action = ActionConfig.ActionList[0];
            EventManager.Broadcast(evtOnChangeTask);
        }
    
    }

    public override void ExitAction()
    {
        EnteredAction = false;
    }
    
    public override void ContinueAction(OnContinueAction onContinueAction)
    {
        if (onContinueAction.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            
        }
    }
        
    public override void OnDiscontinueAction(OnDiscontinueAction onDiscontinueAction)
    {
        if (onDiscontinueAction.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            var evtDiscontinueState = FsmStateEvents.OnDiscontinueState;
            evtDiscontinueState.Sender = CharacterConfig.NpcGeneralConfig.Name;
            EventManager.Broadcast(evtDiscontinueState);
            
            var evtActionExit = FsmActionEvents.OnExitAction;
            evtActionExit.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtActionExit.Exited = true;
            EventManager.Broadcast(evtActionExit);
        }            

    }
    
    public override void OnEnterTaskActionCondition(OnEnterTaskActionCondition onEnterTaskActionCondition)
    {
        if (onEnterTaskActionCondition.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            EnterTaskActionCondition = onEnterTaskActionCondition.Condition;
            ExitTaskActionCondition = false;
        }
    }
    
    public override void OnExitTaskActionCondition(OnExitTaskActionCondition onExitTaskActionCondition)
    {
        if (onExitTaskActionCondition.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            ExitTaskActionCondition = onExitTaskActionCondition.Condition;
            EnterTaskActionCondition = false;
        }
    }
}
