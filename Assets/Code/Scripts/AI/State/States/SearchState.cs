using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class SearchState : BaseState
{
    public SearchState ()
    {
        StatePhase = FsmStatePhase.Search;
    }

    public override void InitializeState()
    {
        base.InitializeState();
    }

    public override void UninitializeState()
    {
        base.UninitializeState();
    }

    public override void InitializeStateEvents()
    {
        base.InitializeStateEvents();

        EventManager.AddListener<OnContinueState>(OnContinueState);
        EventManager.AddListener<OnDiscontinueState>(OnDiscontinueState);
        EventManager.AddListener<OnEnterTaskStateCondition>(OnEnterTaskStateCondition);
        EventManager.AddListener<OnExitTaskStateCondition>(OnExitTaskStateCondition);
        EventManager.AddListener<OnDiscontinueStateToChase>(OnDiscontinueStateToChase);
        EventManager.AddListener<OnDiscontinueStateToCombat>(OnDiscontinueStateToCombat);
    }

    public override void UninitializeStateEvents()
    {
        base.UninitializeStateEvents();

        EventManager.RemoveListener<OnContinueState>(OnContinueState);
        EventManager.RemoveListener<OnDiscontinueState>(OnDiscontinueState);
        EventManager.RemoveListener<OnEnterTaskStateCondition>(OnEnterTaskStateCondition);
        EventManager.RemoveListener<OnExitTaskStateCondition>(OnExitTaskStateCondition);
        EventManager.RemoveListener<OnDiscontinueStateToChase>(OnDiscontinueStateToChase);
        EventManager.RemoveListener<OnDiscontinueStateToCombat>(OnDiscontinueStateToCombat);
    }

    public override void EnterState()
    {
        base.EnterState();

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
            base.UpdateState();
            
            if (AtDestination())
            {
                var evtDiscontinueAction = FsmActionEvents.OnDiscontinueAction;
                evtDiscontinueAction.Sender = CharacterConfig.NpcGeneralConfig.Name;
                EventManager.Broadcast(evtDiscontinueAction);
            }
        }

        if (ExitTaskStateCondition)
        {
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.StateSender = CharacterConfig.NpcGeneralConfig.Name;
            EventManager.Broadcast(evtOnChangeTask);
        }

    }

    private bool AtDestination()
    {
        if (CharacterConfig.NavmeshConfig.Agent.remainingDistance <= 2)
        {
            return true;
        }
        
        return false;
    }
    
    
    
    public override void ExitState()
    {
        base.ExitState();

        EnteredState = false;
    }

    public override void OnContinueState(OnContinueState onContinueState)
    {
        if (onContinueState.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            base.OnContinueState(onContinueState);
        }
    }

    public override void OnDiscontinueState(OnDiscontinueState onDiscontinueState)
    {
        if (onDiscontinueState.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            base.OnDiscontinueState(onDiscontinueState);

            var evtOnHeard = SensorEvents.OnTargetHeard;
            evtOnHeard.Heard = false;
            evtOnHeard.Sender = CharacterConfig.NpcGeneralConfig.Name;
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.state = StateConfig.StateList[0];
            EventManager.Broadcast(evtOnChangeTask);
            
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
            base.OnEnterTaskStateCondition(onEnterTaskStateCondition);

            EnterTaskStateCondition = onEnterTaskStateCondition.Condition;
            ExitTaskStateCondition = false;
        }
    }

    public override void OnExitTaskStateCondition(OnExitTaskStateCondition onExitTaskStateCondition)
    {
        if (onExitTaskStateCondition.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            base.OnExitTaskStateCondition(onExitTaskStateCondition);

            ExitTaskStateCondition = onExitTaskStateCondition.Condition;
            EnterTaskStateCondition = false;
        }
    }
    
    private void OnDiscontinueStateToChase (OnDiscontinueStateToChase  onDiscontinueStateToChase)
    {
        if (onDiscontinueStateToChase.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            //Debug.Log(onDiscontinueStateToChase.Sender + ": On Discontinue State To Chase");
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.state = StateConfig.StateList[2];
            EventManager.Broadcast(evtOnChangeTask);
            
            var evtExit = FsmStateEvents.OnExitState;
            evtExit.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtExit.Exited = true;
            EventManager.Broadcast(evtExit);
        }
    }
    
    private void OnDiscontinueStateToCombat(OnDiscontinueStateToCombat onDiscontinueStateToCombat)
    {
        if ( onDiscontinueStateToCombat.Sender == CharacterConfig.NpcGeneralConfig.Name)
        {
            //Debug.Log( onDiscontinueStateToCombat.Sender + ": On Discontinue State To Combat");
            
            var evtOnChangeTask = AITaskEvents.OnChangeTask;
            evtOnChangeTask.state = StateConfig.StateList[3];
            EventManager.Broadcast(evtOnChangeTask);
            
            var evtStateExit = FsmStateEvents.OnExitState;
            evtStateExit.Sender = CharacterConfig.NpcGeneralConfig.Name;
            evtStateExit.Exited = true;
            EventManager.Broadcast(evtStateExit);
        }
    }
}