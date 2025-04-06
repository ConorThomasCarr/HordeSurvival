using System;

using AI.BaseCharacters.Humans.BaseHunter;
using AI.BaseCharacters.Humans.BaseHunter.Hunter;

namespace AI.BaseCharacters.Humans
{
      public class Human : IHuman
      {
            public BaseHunter.BaseHunter CreateHunter() => new Hunter();
      }
}     