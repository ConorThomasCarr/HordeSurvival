using AI.BaseNpc;
using AI.Config.Character;

using UnityEngine;
using UnityEngine.Events;

namespace AI.BaseCharacters.Undead.BaseZombie
{
    public abstract class BaseZombie : ICharacters
    {
        public UnityAction<FsmStatePhase, FsmActionPhase> OnTaskChanged { get; set; }
        
        public CharacterConfig CharConfig { get; set; }
        
        #region Initialize

        public virtual void InitializeEvents() { }
        
        public virtual void InitializeNpc(INpc iNpc) { }
        
        public virtual void InitializeConfig(CharacterConfig characterConfig) { }

        #endregion Initialize
        
        #region Task Management

        public virtual bool ContinueTaskCondition(){ return false; }

        public virtual bool EnterTaskCondition(){ return false; }

        public virtual bool ExitTaskCondition(){ return false; }

        #endregion Task Management
        
        #region State Management

        public virtual bool EnteredState(){ return false; }

        public virtual bool ExitedState() { return false; }

        #endregion State Management
        
        #region Action Management

        public virtual bool EnteredAction(){ return false; }

        public virtual bool ExitedAction(){ return false; }

        #endregion Action Management
        
        #region Checks

        public virtual bool CheckSenderName(string name){ return false; }

        public virtual bool AgentHasPath(){ return false; }

        #endregion Checks
        
        #region AI
        
        public virtual void SetDestination(Vector3 destination){ }

        #endregion AI
    }
}

