using UnityEngine;

using AI.BaseCharacters.Humans.BaseHunter;

namespace AI.BaseCharacters.Humans
{
    public interface IHuman
    {
        public BaseHunter.BaseHunter CreateHunter();
    }
}


