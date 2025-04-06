using System;
using System.Collections.Generic;

namespace AI.Config.States
{
    public struct StateConfig
    {
        public StateConfig(List<FsmStatePhase> stateList)
        {
            StateList = stateList;
        }
         
        public List<FsmStatePhase> StateList {get; set;}
    }
}


