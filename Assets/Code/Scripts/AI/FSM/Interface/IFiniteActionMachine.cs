using System.Collections.Generic;
using AI.Config.Actions;
using AI.Config.Character;
using UnityEngine.Events;

public interface IFiniteActionMachine
{
 IActions CurrentAction { get; set; }

 List<IActions> ValidAction { get; set; }

 Dictionary<FsmActionPhase, IActions> FsmActions { get; set; }

 UnityAction InitializeFiniteActionMachine { get; set; }
 UnityAction UninitializeFiniteActionMachine { get; set; }
 UnityAction<ActionConfig, CharacterConfig> InitializeActionConfig { get; set; }
 void Awake();
 
 void Enable();

 void Disable();

 void Update();

 void EnterAction(IActions nextAction);

 void EnterActionPhase(FsmActionPhase actionType);

}
