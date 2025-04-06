using AI.BaseNpc;
using AI.Config.Character;
using UnityEngine;
using UnityEngine.Events;

namespace AI.BaseCharacters
{
    public interface ICharacters
    {
        UnityAction<FsmStatePhase, FsmActionPhase> OnTaskChanged { get; set; } 
        CharacterConfig CharConfig { get; set; }

        #region Initialize

        void InitializeEvents();
        
        void InitializeNpc(INpc iNpc);

        void InitializeConfig(CharacterConfig characterConfig);

        #endregion Initialize

        #region Task Management

        bool ContinueTaskCondition();

        bool EnterTaskCondition();

        bool ExitTaskCondition();

        #endregion Task Management

        #region State Management

        bool EnteredState();

        bool ExitedState();

        #endregion State Management

        #region Action Management

        bool EnteredAction();

        bool ExitedAction();

        #endregion Action Management

        #region Checks

        bool CheckSenderName(string name);

        bool AgentHasPath();

        #endregion Checks

        #region AI

        void SetDestination(Vector3 destination);

        #endregion AI

    }
}


