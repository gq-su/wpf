using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LcrSimulator.Model
{
    public class Dice
    {
        public string[] DiceFaces { get; private set; } = new string[] { "L", "D", "C", "D", "R", "D" };

        public string DiceFace { get; set; }

        public int Index { get; set; }

        public void Roll(int playerIndex)
        {
            var rand = new Random(Environment.TickCount + playerIndex+Index);
            DiceFace = DiceFaces[rand.Next(0, DiceFaces.Length)];
        }
    }
}
