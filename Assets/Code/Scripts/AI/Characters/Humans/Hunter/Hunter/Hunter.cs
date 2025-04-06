using AI.BaseNpc;
using AI.BaseNpc.Human;
using AI.Config.Character;
using UnityEngine;


namespace AI.BaseCharacters.Humans.BaseHunter.Hunter
{
    public class Hunter : BaseHunter
    {
        private IHumanNpc Npc { get; set; }

        private bool _hasEnteredState;

        private bool _hasExitedState;

        private bool _hasEnteredAction;

        private bool _hasExitedAction;

        #region Initialize

        public override void InitializeNpc(INpc iNpc)
        {
            Npc = (IHumanNpc)iNpc;
            
            Debug.Log(Npc);
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

            EventManager.AddListener<OnDestination>(OnDestination);
            EventManager.AddListener<OnMoveCharacter>(MoveCharacter);

            OnTaskChanged += ChangeTask;

            _hasEnteredState = false;
            _hasExitedState = false;

            _hasEnteredAction = false;
            _hasExitedAction = false;
        }

        public override void InitializeConfig(CharacterConfig characterConfig)
        {
            CharConfig = characterConfig;
            
            Debug.Log(CharConfig);
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
    }

}


