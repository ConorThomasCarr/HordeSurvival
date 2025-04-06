using UnityEngine;
using UnityEngine.AI;

namespace AI.Config.Navmesh
{
    public struct NavmeshConfig
    {
        public NavmeshConfig (NavMeshAgent agent)
        {
            Agent = agent;
        }
    
        public NavMeshAgent Agent { get; set; }
    
    }
}


