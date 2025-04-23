using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CombatState : BaseState
{
    public CombatState ()
    {
        StatePhase = FsmStatePhase.Combat;
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
    }

    public override void UninitializeStateEvents()
    {
        base.UninitializeStateEvents();

        EventManager.RemoveListener<OnContinueState>(OnContinueState);
        EventManager.RemoveListener<OnDiscontinueState>(OnDiscontinueState);
        EventManager.RemoveListener<OnEnterTaskStateCondition>(OnEnterTaskStateCondition);
        EventManager.RemoveListener<OnExitTaskStateCondition>(OnExitTaskStateCondition);
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

            if (AtDestination() && !CharacterConfig.NavmeshConfig.Agent.isStopped)
            {
                CharacterConfig.NavmeshConfig.Agent.isStopped = true;
            }


            if (!AtDestination() && CharacterConfig.NavmeshConfig.Agent.isStopped)
            {
                CharacterConfig.NavmeshConfig.Agent.isStopped = false;
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
        if (CharacterConfig.NavmeshConfig.Agent.remainingDistance <= 5)
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
            
            var evtOnCombat = SensorEvents.OnTargetIsInRange;
            evtOnCombat.Combat = false;
            evtOnCombat.Sender = CharacterConfig.NpcGeneralConfig.Name;
            EventManager.Broadcast(evtOnCombat);
            
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
}
