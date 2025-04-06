using UnityEngine;

namespace AI.Config.NpcGeneral
{
    public struct NpcGeneralConfig
    {
        public NpcGeneralConfig(string name)
        {
            _name = name;
        }
        

        private readonly string _name;
        
        public string Name {
            
            get => _name;
            set => Name = value;
        }
    }
}


