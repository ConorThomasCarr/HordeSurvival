using AI.BaseCharacters.Undead.BaseZombie.Zombie;

namespace AI.BaseCharacters.Undead
{
    public class Undead : IUndead
    {
        public BaseZombie.BaseZombie CreateZombie() => new Zombie();
    }
}