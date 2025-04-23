using AI.BaseNpc;

using AI.BaseNpc.Undead;
using AI.Config.Character;

using UnityEngine;


namespace AI.BaseCharacters.Undead.BaseZombie.Zombie
{
    public class Zombie : BaseZombie
    {
        private IUndeadNpc Npc { get; set; }
        
        private GenerateRandomSearchDestination Grsd { get; set; }
        
        private bool _hasEnteredState;

        private bool _hasExitedState;

        private bool _hasEnteredAction;
        
        private bool _hasExitedAction;
        
        private bool _sightedPlayer;
        
        private bool _targetIsInRange;
        
        public override void InitializeEvents()
        {
            EventManager.AddListener<OnExitState>(ExitState);
            EventManager.AddListener<OnEnterState>(EnterState);

            EventManager.AddListener<OnExitAction>(ExitAction);
            EventManager.AddListener<OnEnterAction>(EnterAction);
            
            EventManager.AddListener<OnChangeTask>(OnChangeTask);
            
            EventManager.AddListener<OnGrsdGenerateArrayRowAndColum>(OnGrsdGenerateArrayRowAndColum);    
            EventManager.AddListener<OnGrsdClearArrayRowAndColum>(OnGrsdClearArrayRowAndColum);
            
            EventManager.AddListener<OnTargetSighted>(SightedTarget);
            EventManager.AddListener<OnTargetIsInRange>(TargetIsInRange);
            
            _hasEnteredState = false;
            _hasExitedState = false;

            _hasEnteredAction = false;
            _hasExitedAction = false;
            
            _sightedPlayer = false;
            _targetIsInRange = false;
        }

        public override void InitializeNpc(INpc iNpc)
        {
            Npc = (IUndeadNpc)iNpc;
        }

        
        public override void InitializeConfig(CharacterConfig characterConfig)
        {
            CharConfig = characterConfig;
            
            Grsd = new GenerateRandomSearchDestination();
           
            Grsd.Initialize(this);
        }

        
        private void EnterState(OnEnterState enterState)
        {
            if (CheckSenderName(enterState.Sender))
            {
                _hasEnteredState = enterState.Enter;
                _hasExitedState = false;

                var evtOnEnterTaskStateCondition = FsmStateEvents.OnEnterTaskStateCondition;
                evtOnEnterTaskStateCondition.Sender = CharConfig.NpcGeneralConfig.Name;
                evtOnEnterTaskStateCondition.Condition = true;
                EventManager.Broadcast(evtOnEnterTaskStateCondition);
            }
        }

        private void ExitState(OnExitState exitState)
        {
            if (CheckSenderName(exitState.Sender))
            {
                _hasExitedState = exitState.Exited;
                _hasEnteredState = false;

                var evtOnExitTaskStateCondition = FsmStateEvents.OnExitTaskStateCondition;
                evtOnExitTaskStateCondition.Sender = CharConfig.NpcGeneralConfig.Name;
                evtOnExitTaskStateCondition.Condition = true;
                EventManager.Broadcast(evtOnExitTaskStateCondition);


            }
        }
        
        private void EnterAction(OnEnterAction enterAction)
        {
            if (CheckSenderName(enterAction.Sender))
            {
                _hasEnteredAction = enterAction.Enter;
                _hasExitedAction = false;

                var evtOnEnterTaskActionCondition = FsmActionEvents.OnEnterTaskActionCondition;
                evtOnEnterTaskActionCondition.Sender = CharConfig.NpcGeneralConfig.Name;
                evtOnEnterTaskActionCondition.Condition = true;
                EventManager.Broadcast(evtOnEnterTaskActionCondition);
            }
        }

        private void ExitAction(OnExitAction exitAction)
        {
            if (CheckSenderName(exitAction.Sender))
            {
                _hasExitedAction = exitAction.Exited;
                _hasEnteredAction = false;

                var evtOnExitTaskActionCondition = FsmActionEvents.OnExitTaskActionCondition;
                evtOnExitTaskActionCondition.Sender = CharConfig.NpcGeneralConfig.Name;
                evtOnExitTaskActionCondition.Condition = true;
                EventManager.Broadcast(evtOnExitTaskActionCondition);
            }
        }
        
        private void OnChangeTask(OnChangeTask onChangeTask)
        {
            if (CheckSenderName(onChangeTask.StateSender)
                && CheckSenderName(onChangeTask.ActionSender) 
                && onChangeTask.action != FsmActionPhase.None && onChangeTask.state != FsmStatePhase.None)
            {
                Npc.StatePhaseChanged?.Invoke(onChangeTask.state);
                Npc.ActionPhaseChanged?.Invoke(onChangeTask.action);
            }
        }
        
        
        private void OnGrsdGenerateArrayRowAndColum(OnGrsdGenerateArrayRowAndColum onGrsdGenerateArrayRowAndColum)
        {
            if (CheckSenderName(onGrsdGenerateArrayRowAndColum.Sender))
            {
                Grsd.GenerateArrayRowAndColumn();
            }
        }
        
        private void OnGrsdClearArrayRowAndColum(OnGrsdClearArrayRowAndColum onGrsdClearArrayRowAndColum)
        {
            if (CheckSenderName(onGrsdClearArrayRowAndColum.Sender))
            {
                Grsd.ClearArrayRowAndColumn();
            }
        }

        public override bool CheckSenderName(string name)
        {
            return (name == CharConfig.NpcGeneralConfig.Name);
        }

        public override bool AgentHasPath()
        {
            return false;
        }

        public override void SetDestination(Vector3 destination)
        {
            if (CharConfig.NavmeshConfig.Agent.destination != destination)
            {
                CharConfig.NavmeshConfig.Agent.destination = destination;
            }
        }
        
        private void SightedTarget(OnTargetSighted sighted)
        {
            if (CheckSenderName(sighted.Sender) && !_sightedPlayer && !_targetIsInRange)
            {
                //Debug.LogError("Sighted Target: " +  sighted.Sender);
                
                _sightedPlayer = sighted.Sighted;
               
                SetDestination(sighted.SightedLocation);
                
                var evtOnDiscontinueActionToChase = FsmActionEvents.OnDiscontinueActionToChase;
                evtOnDiscontinueActionToChase.Sender = CharConfig.NpcGeneralConfig.Name;
                EventManager.Broadcast(evtOnDiscontinueActionToChase);
            }
            
            if (CheckSenderName(sighted.Sender) && _sightedPlayer && !_targetIsInRange)
            {
                SetDestination(sighted.SightedLocation);
            }
        }

        private void TargetIsInRange(OnTargetIsInRange isInRange)
        {
            if (CheckSenderName(isInRange.Sender) && !_targetIsInRange)
            {
                //Debug.LogError("Target Is In Range: " + isInRange.Sender);

                var evtOnDiscontinueActionToCombat = FsmActionEvents.OnDiscontinueActionToCombat;
                evtOnDiscontinueActionToCombat.Sender = CharConfig.NpcGeneralConfig.Name;
                EventManager.Broadcast(evtOnDiscontinueActionToCombat);

                SetTarget(isInRange.Target);

                SetDestination(isInRange.Target.transform.position);

                _targetIsInRange = true;
            }

            if (CheckSenderName(isInRange.Sender) && _targetIsInRange)
            {
                SetDestination(Player.transform.position);
            }
        }
        
        private void SetTarget(GameObject target)
        {
            if (target != null)
            {
                Player = target;
            }
        }
        
 
    }

}


