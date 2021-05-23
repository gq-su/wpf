using System;
using System.Windows;

namespace LcrSimulator.Model
{
    public class Dice
    {
        public char[] DiceFaces { get; private set; } = new char[] { 'L', 'D', 'C', 'D', 'R', 'D' };

        public char DiceFace { get; set; }

        /// <summary>
        /// Indicates is used in game when its value is true
        /// </summary>
        public bool IsActive { get; set; }

        public void Roll()
        {
            var rand = Application.Current.Properties["Rand"] as Random;
            DiceFace = DiceFaces[rand.Next(0, DiceFaces.Length)];
            IsActive = true;
        }
    }
}
