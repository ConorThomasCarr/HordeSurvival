using AI.BaseCharacters;

using UnityEngine.Events;

namespace AI.BaseNpc
{
    public interface INpc
    {
        UnityAction InitializeConstruction { get; set; }
        UnityAction<FsmStatePhase> StatePhaseChanged { get; set; }
        UnityAction<FsmActionPhase> ActionPhaseChanged { get; set; }
        UnityAction <ICharacters> InitializeNpc { get; set; } 
        UnityAction InitializeConfigs { get; set; }
        UnityAction InitializeFsmSystem{ get; set; } 
        
        UnityAction InitializeWeapons{ get; set; }

        void Enable();
        
        void Disable();
    }
}


