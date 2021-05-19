using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LcrSimulator.Model
{
    public class Player : BindableBase
    {
        public readonly Dices Dices;
        public Player()
        {
            Dices = Application.Current.Properties["Dices"] as Dices;
            _chipsCount = Dices.MaxDiceCount;
        }

        public int Index { get; set; }

        private int _chipsCount;
        public int ChipsCount
        {
            get { return _chipsCount; }
            set
            {
                if (_chipsCount == value)
                    return;

                _chipsCount = value < 0 ? 0 : value;
            }
        }

        public void Play(int seed)
        {
            if (ChipsCount <= 0) return;

            var playDiceCount = ChipsCount > Dices.MaxDiceCount ? Dices.MaxDiceCount : ChipsCount;

            Dices.ForEach(o => o.IsActive = false);

            for (int i = 0; i < playDiceCount; i++)
            {
                Dices[i].Roll(Index + seed);
            }

            RaisePropertyChanged("Played");
            //Thread.Sleep(100);
        }
    }
}
