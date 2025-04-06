using AI.BaseNpc;

using AI.BaseNpc.Undead;
using AI.Config.Character;

using UnityEngine;


namespace AI.BaseCharacters.Undead.BaseZombie.Zombie
{
    public class Zombie : BaseZombie
    {
        private IUndeadNpc Npc { get; set; }

        private bool _hasEnteredState;

        private bool _hasExitedState;

        private bool _hasEnteredAction;

        private bool _hasExitedAction;
        
        private GenerateRandomSearchDestination Grsd { get; set; }

        private bool _heardPlayer;

        private bool _sightedPlayer;
        
        private bool _targetIsInRange;

        #region Initialize

        public override void InitializeNpc(INpc iNpc)
        {
            Npc = (IUndeadNpc)iNpc;
        }

        public override void InitializeEvents()
        {
            EventManager.AddListener<OnExitState>(ExitState);
            EventManager.AddListener<OnEnterState>(EnterState);
            
            EventManager.AddListener<OnExitAction>(ExitAction);
            EventManager.AddListener<OnEnterAction>(EnterAction);
            
            EventManager.AddListener<OnChangeTask>(OnChangeTask);
            
            EventManager.AddListener<OnContinueTask>(ContinueTask);
            EventManager.AddListener<OnDiscontinueTask>(DiscontinueTask);
            
            EventManager.AddListener<OnTargetHeard>(HeardPlayerNoise);
            EventManager.AddListener<OnTargetSighted>(SightedTarget);
            EventManager.AddListener<OnTargetIsInRange>(TargetIsInRange);

            EventManager.AddListener<OnGrsdGenerateArrayRowAndColum>(OnGrsdGenerateArrayRowAndColum);    
            EventManager.AddListener<OnGrsdClearArrayRowAndColum>(OnGrsdClearArrayRowAndColum);
           
            OnTaskChanged += ChangeTask;
            
            _hasEnteredState = false;
            _hasExitedState = false;
            
            _hasEnteredAction = false;
            _hasExitedAction = false;
        }

        public override void InitializeConfig(CharacterConfig characterConfig)
        {
            CharConfig = characterConfig;
            
            Grsd = new GenerateRandomSearchDestination();
            Grsd.Initialize(this);
        }

        #endregion Initialize

        #region Task Management

        private void ContinueTask(OnContinueTask onContinueTask)
        {
            if (CheckSenderName(onContinueTask.Sender))
            {
                if (ContinueTaskCondition())
                {
                    var evtContinueAction = FsmActionEvents.OnContinueAction;
                    evtContinueAction.Sender = CharConfig.NpcGeneralConfig.Name;
                    EventManager.Broadcast(evtContinueAction);
                }
            }
        }

        private void OnChangeTask(OnChangeTask onChangeTask)
        {
            if (CheckSenderName(onChangeTask.StateSender)
                && CheckSenderName(onChangeTask.ActionSender)
                && onChangeTask.action != FsmActionPhase.None
                && onChangeTask.state != FsmStatePhase.None)
            {
                Npc.StatePhaseChanged?.Invoke(onChangeTask.state);
                Npc.ActionPhaseChanged?.Invoke(onChangeTask.action);
            }
        }

        private void ChangeTask(FsmStatePhase nextPhase, FsmActionPhase nextAction)
        {
            Npc.StatePhaseChanged?.Invoke(nextPhase);
            Npc.ActionPhaseChanged?.Invoke(nextAction);
        }

        public override bool ContinueTaskCondition()
        {
            var continueCondition = (!AgentHasPath());
            return continueCondition;
        }

        private bool DiscontinueTaskCondition()
        {
            var discontinueCondition = (ExitedState() && ExitedAction());
            return discontinueCondition;
        }

        private void DiscontinueTask(OnDiscontinueTask onDiscontinueTask)
        {
            if (CheckSenderName(onDiscontinueTask.Sender) && DiscontinueTaskCondition())
            {
                ChangeTask(FsmStatePhase.Idle, FsmActionPhase.Explore);
            }
        }

        public override bool EnterTaskCondition()
        {
            var enterCondition = (EnteredState() && EnteredAction());
            return enterCondition;
        }

        public override bool ExitTaskCondition()
        {
            var exitCondition = (ExitedState() && ExitedAction());
            return exitCondition;
        }

        #endregion Task Management

        #region State Management

        private void EnterState(OnEnterState onEnter)
        {
            if (CheckSenderName(onEnter.Sender))
            {
                _hasEnteredState = onEnter.Enter;
                _hasExitedState = false;

                var evtOnEnterTaskStateCondition = FsmStateEvents.OnEnterTaskStateCondition;
                evtOnEnterTaskStateCondition.Sender = CharConfig.NpcGeneralConfig.Name;
                evtOnEnterTaskStateCondition.Condition = true;
                EventManager.Broadcast(evtOnEnterTaskStateCondition);
            }
        }

        private void ExitState(OnExitState onExit)
        {
            if (CheckSenderName(onExit.Sender))
            {
                _hasExitedState = onExit.Exited;
                _hasEnteredState = false;

                var evtOnExitTaskStateCondition = FsmStateEvents.OnExitTaskStateCondition;
                evtOnExitTaskStateCondition.Sender = CharConfig.NpcGeneralConfig.Name;
                evtOnExitTaskStateCondition.Condition = true;
                EventManager.Broadcast(evtOnExitTaskStateCondition);


            }
        }

        public override bool EnteredState()
        {
            return _hasEnteredState;
        }

        public override bool ExitedState()
        {
            return _hasExitedState;
        }

        #endregion State Management

        #region Action Management

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

        public override bool EnteredAction()
        {
            return _hasEnteredAction;
        }

        public override bool ExitedAction()
        {
            return _hasExitedAction;
        }

        #endregion Action Management

        private void MoveCharacter(OnMoveCharacter moveTo)
        {
          SetDestination(moveTo.Position);
        }
        
        public override void SetDestination(Vector3 destination)
        {
            if (CharConfig.NavmeshConfig.Agent.destination != destination)
            {
                if (!CharConfig.NavmeshConfig.Agent.hasPath)
                {
                    var evtDiscontinueAction = FsmActionEvents.OnDiscontinueAction;
                    evtDiscontinueAction.Sender = CharConfig.NpcGeneralConfig.Name;
                    EventManager.Broadcast(evtDiscontinueAction);

                }

                CharConfig.NavmeshConfig.Agent.destination = destination;
                
            }
        }

        private void OnDestination(OnDestination onDestination)
        {
            if (CheckSenderName(onDestination.Sender))
            {
                CharConfig.NavmeshConfig.Agent.isStopped = false;
            }
        }

        public override bool CheckSenderName(string name)
        {
            var sender = (CharConfig.NpcGeneralConfig.Name == name);

            return sender;
        }

        public override bool AgentHasPath()
        {
            var hasPath = (CharConfig.NavmeshConfig.Agent.hasPath);

            return hasPath;
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
        
        
        private void HeardPlayerNoise(OnTargetHeard noise)
        {
            if (CheckSenderName(noise.Sender) && !_sightedPlayer && !_heardPlayer && !_targetIsInRange)
            {
                var evtOnDiscontinueActionToSearch = FsmActionEvents.OnDiscontinueActionToSearch;
                evtOnDiscontinueActionToSearch.Sender = CharConfig.NpcGeneralConfig.Name;
                EventManager.Broadcast(evtOnDiscontinueActionToSearch);
                
                _heardPlayer = noise.Heard;
                SetDestination(noise.SoundLocation);
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

                _targetIsInRange = isInRange.Combat;
            }
            
            if (CheckSenderName(isInRange.Sender) && _targetIsInRange)
            {
                SetDestination(isInRange.Target.transform.position);
            }
        }


        public bool HeardTarget()
        {
            return  _heardPlayer;
        }

        public bool SightedTarget()
        {
            return _sightedPlayer;
        }

        public bool TargetInRange()
        {
            return _targetIsInRange;
        }

        public bool GrsdHasGenerated()
        {
            var hasGenerated = (Grsd.hasGenerateArrayRowAndColum);
            
            return hasGenerated;
        }

        public bool GrsdHasCleared()
        {
            var hasCleared = (Grsd.hasClearArrayRowAndColumn);
            
            return hasCleared;
        }
        
        private void SetTarget(GameObject target)
        {
            if (target != null)
            {
                Player = target;
            }
        }
        
        public GameObject Player {get;set;}
    }
}


