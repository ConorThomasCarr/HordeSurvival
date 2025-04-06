using System.Collections.Generic;
using AI.Config.Character;
using AI.Config.States;
using UnityEngine.Events;

public interface IFiniteStateMachine
{
 IStates CurrentState { get; set; }

 List<IStates> ValidStates { get; set; }

 Dictionary<FsmStatePhase, IStates> FsmStates { get; set; }

 UnityAction InitializeFiniteStateMachine { get; set; }
 UnityAction UninitializeFiniteStateMachine { get; set; }
 UnityAction<StateConfig, CharacterConfig> InitializeStateConfig { get; set; }

 void Awake();

 void Enable();

 void Disable();

 void Update();

 void EnterState(IStates nextState);

 void EnterStatePhase(FsmStatePhase stateType);
}
