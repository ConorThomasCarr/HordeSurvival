using System;
using AI.BaseCharacters.Undead;
using AI.BaseNpc;
using AI.BaseNpc.Undead;


namespace AI.UndeadConstructors
{
    public class UndeadWorld<T> : Undead where T : IUndead, new()
    {
        public UndeadWorld(UndeadType type, INpc npc)
        {
            switch (type)
            {
                case UndeadType.Zombie:

                    var zombie = new T();
                    var enemyZombie = zombie.CreateZombie();
                    var zombieNpc = (IUndeadNpc)npc;
                    
                    zombieNpc.InitializeNpc?.Invoke(enemyZombie);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}