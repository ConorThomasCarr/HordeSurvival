using System.Collections.Generic;

namespace AI.Config.Actions
{
    public struct ActionConfig
    {
        public ActionConfig(List<FsmActionPhase> actionList)
        {
            ActionList = actionList;
        }
     
        public List<FsmActionPhase> ActionList {get; set;}
    }
}


