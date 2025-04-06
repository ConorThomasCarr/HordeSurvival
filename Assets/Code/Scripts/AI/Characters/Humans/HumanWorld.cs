using System;
using AI.BaseCharacters.Humans;
using AI.BaseNpc;
using AI.BaseNpc.Human;

namespace AI.HumanConstructors
{
    public class HumanWorld<T> : Human where T : IHuman, new()
    {
        public HumanWorld(HumanTypes type, INpc npc)
        {
            switch (type)
            {
                case HumanTypes.Hunter:

                    var hunter = new T();
                    var playerHunter = hunter.CreateHunter();
                    var hunterNpc = (IHumanNpc)npc;
                    
                    hunterNpc.InitializeNpc?.Invoke(playerHunter);
                    

                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}

