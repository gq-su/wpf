using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LcrSimulator.Model
{
    public class Player : BindableBase
    {
        const int MaxDiceCount = 3;

        public Player()
        {
            _chipsCount = MaxDiceCount;

            for(int i = 0; i < MaxDiceCount; i++)
            {
                var dice = new Dice { Index = i };
                Dices.Add(dice);
            }
        }

        public int Index { get; set; }

        private int _chipsCount;
        public int ChipsCount
        {
            get { return _chipsCount; }
            set
            {
                if (_chipsCount == value) return;

                var oldCount = _chipsCount;
                _chipsCount = value < 0 ? 0 : value;

                AdjustDiceList(oldCount, _chipsCount);
            }
        }

        public List<Dice> Dices { get; private set; } = new List<Dice>();

        public void Play()
        {
            if (Dices.Count < 1)
                return;
            
            Dices.ForEach((dice) => dice.Roll(Index));

            RaisePropertyChanged("Played");
        }

        private void RemoveDice()
        {
            if (Dices.Any())
                Dices.RemoveAt(Dices.Count - 1);
        }

        private void AddDice()
        {
            if (Dices.Count >= MaxDiceCount)
                return;

            Dices.Add(new Dice{ Index = Dices.Count });
        }

        private void AdjustDiceList(int oldChipsCount, int newChipsCount)
        {
            bool removeDice = false;
            int countChange;
            if (newChipsCount>oldChipsCount)
               countChange = newChipsCount - oldChipsCount;
            else
            {
                if (oldChipsCount > MaxDiceCount)
                    countChange = MaxDiceCount - newChipsCount;
                else
                    countChange = oldChipsCount - newChipsCount;

                removeDice = true;
            }

            for (int i = 0; i < Math.Abs(countChange); i++)
            {
                if (removeDice)
                    RemoveDice();
                else
                    AddDice(); 
            }
        }
    }
}
