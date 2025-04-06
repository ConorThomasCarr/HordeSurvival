using AI.Config.NpcGeneral;
using AI.Config.Navmesh;

namespace AI.Config.Character
{
    public struct CharacterConfig
    {
        public CharacterConfig(NavmeshConfig navmeshConfig, NpcGeneralConfig npcGeneralConfig)
        {
            _navmeshConfig = navmeshConfig;
            _npcGeneralConfig = npcGeneralConfig;
        }

        private readonly NavmeshConfig _navmeshConfig;
        private readonly NpcGeneralConfig _npcGeneralConfig;
        
        public NavmeshConfig NavmeshConfig { 
            get => _navmeshConfig;
            set => NavmeshConfig = value;
        }
        public NpcGeneralConfig NpcGeneralConfig { 
            get => _npcGeneralConfig;
            set => NpcGeneralConfig = value;
        }
    }
}


