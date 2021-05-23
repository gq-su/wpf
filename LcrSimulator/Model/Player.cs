using Prism.Mvvm;
using System.Windows;

namespace LcrSimulator.Model
{
    public class Player : BindableBase
    {
        public readonly Dices Dices;

        public Player(int initChipsCount)
        {
            Dices = Application.Current.Properties["Dices"] as Dices;
            ChipsCount = initChipsCount;
        }

        public int Index { get; set; }

        private int _chipsCount;
        public int ChipsCount
        {
            get { return _chipsCount; }
            set { _chipsCount = value < 0 ? 0 : value; }
        }

        public void Play()
        {
            if (ChipsCount <= 0) return;

            var playDiceCount = ChipsCount > Dices.MaxDiceCount ? Dices.MaxDiceCount : ChipsCount;

            Dices.ForEach(o => o.IsActive = false);

            for (int i = 0; i < playDiceCount; i++)
            {
                Dices[i].Roll();
            }

            RaisePropertyChanged("Played");
        }
    }
}
