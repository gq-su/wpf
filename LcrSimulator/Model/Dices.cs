using System.Collections.Generic;

namespace LcrSimulator.Model
{
    public class Dices : List<Dice>
    {
        public const int MaxDiceCount = 3;

        public Dices()
        {
            for (int i = 0; i < MaxDiceCount; i++)
            {
                Add(new Dice());
            }
        }
    }
}
