using UnityEngine;

public class ChaseAction : BaseActions
{
    public ChaseAction()
    {
        ActionPhase = FsmActionPhase.Chase;
    }
    
    public override void InitializeAction()
    {
        base.InitializeAction();
    }

    public override void UninitializeAction()
    {
        base.UninitializeAction();
    }

    public override void InitializeActionEvents()
    {
        base.InitializeActionEvents();
        
        EventManager.AddListener<OnContinueAction>(ContinueAction);
        EventManager.AddListener<OnDiscontinueAction>(OnDiscontinueAction);
        EventManager.AddListener<OnEnterTaskActionCondition>(OnEnterTaskActionCondition);
        EventManager.AddListener<OnExitTaskActionCondition>(OnExitTaskActionCondition);
        EventManager.AddListener<OnDiscontinueActionToCombat>(OnDiscontinueActionToCombat);
    }

    public override void UninitializeActionEvents()
    {
        base.UninitializeActionEvents();
        
        EventManager.RemoveListener<OnContinueAction>(ContinueAction);
        EventManager.RemoveListener<OnDiscontinueAction>(OnDiscontinueAction);
        EventManager.RemoveListener<OnEnterTaskActionCondition>(OnEnterTaskActionCondition);
        EventManager.RemoveListener<OnExitTaskActionCondition>(OnExitTaskActionCondition);
        EventManager.RemoveListener<OnDiscontinueActionToCombat>(OnDiscontinueActionToCombat);
    }

    public override void EnterAction()
    {
        base.EnterAction();
        
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
            base.UpdateAction();
        }

        if (ExitTaskActionCondition)
        {
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.ActionSender = CharacterConfig.NpcGeneralConfig.Name;
            EventManager.Broadcast(evtOnChangeTask);
        }
    
    }

    public override void ExitAction()
    {
        base.ExitAction();
        EnteredAction = false;
    }
    
    public override void ContinueAction(OnContinueAction onContinueAction)
    {
        if (onContinueAction.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            base.ContinueAction(onContinueAction);
        }
    }
        
    public override void OnDiscontinueAction(OnDiscontinueAction onDiscontinueAction)
    {
        if (onDiscontinueAction.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            base.OnDiscontinueAction(onDiscontinueAction);
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.action = ActionConfig.ActionList[0];
            EventManager.Broadcast(evtOnChangeTask);
            
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
            base.OnEnterTaskActionCondition(onEnterTaskActionCondition);

            EnterTaskActionCondition = onEnterTaskActionCondition.Condition;
            ExitTaskActionCondition = false;
        }
    }
    
    public override void OnExitTaskActionCondition(OnExitTaskActionCondition onExitTaskActionCondition)
    {
        if (onExitTaskActionCondition.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            base.OnExitTaskActionCondition(onExitTaskActionCondition);

            ExitTaskActionCondition = onExitTaskActionCondition.Condition;
            EnterTaskActionCondition = false;
        }
    } 
    
    private void OnDiscontinueActionToCombat(OnDiscontinueActionToCombat onDiscontinueActionToCombat)
    {
        if ( onDiscontinueActionToCombat.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            Debug.Log( onDiscontinueActionToCombat.Sender + ": On Discontinue Action To Combat");
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.action = ActionConfig.ActionList[3];
            EventManager.Broadcast(evtOnChangeTask);
            
            var evtOnDiscontinueStateToCombat = FsmStateEvents.OnDiscontinueStateToCombat;
            evtOnDiscontinueStateToCombat.Sender = CharacterConfig.NpcGeneralConfig.Name;
            EventManager.Broadcast(evtOnDiscontinueStateToCombat);
            
            var evtActionExit = FsmActionEvents.OnExitAction;
            evtActionExit.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtActionExit.Exited = true;
            EventManager.Broadcast(evtActionExit);
        }
    }
}
