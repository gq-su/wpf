using LcrSimulator.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LcrSimulator
{
    internal class MainWindowViewModel : BindableBase, IDataErrorInfo
    {
        public MainWindowViewModel(LcrGame lcrGame)
        {
            LcrGame = lcrGame;
            PlayGameCommand = new DelegateCommand(PlayGameCommandHandlerAsync, CanExecutePlayGame);
        }

        public LcrGame LcrGame { get; set; }

        private string _title = "LCR Game Simulator";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _playersCount = LcrGame.MinPlayersCount;
        public int PlayersCount
        {
            get { return _playersCount; }
            set
            {
                SetProperty(ref _playersCount, value);
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersCount"));
            }
        }

        private int _gamesCount = LcrGame.MinGamesCount;
        public int GamesCount
        {
            get { return _gamesCount; }
            set
            {
                SetProperty(ref _gamesCount, value);
                OnPropertyChanged(new PropertyChangedEventArgs("GamesCount"));
            }
        }

        private int _minTurns;
        public int MinTurns
        {
            get { return _minTurns; }
            set { SetProperty(ref _minTurns, value); }
        }

        private int _maxTurns;
        public int MaxTurns
        {
            get { return _maxTurns; }
            set { SetProperty(ref _maxTurns, value); }
        }

        private double _avgTurns;
        public double AvgTurns
        {
            get { return _avgTurns; }
            set { SetProperty(ref _avgTurns, value); }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "PlayersCount":
                case "GamesCount":
                    CanExecutePlayGame();
                    break;
                default:
                    break;
            }
        }

        #region PlayGameCommand
        public DelegateCommand PlayGameCommand { get; private set; }

        private bool CanExecutePlayGame()
        {
            return PlayersCount > LcrGame.MinPlayersCount - 1 && GamesCount > LcrGame.MinGamesCount - 1;
        }

        private async void PlayGameCommandHandlerAsync()
        {
            MinTurns = MaxTurns = 0;
            AvgTurns = 0.0;

            LcrGame.GamesCount = GamesCount;
            LcrGame.PlayersCount = PlayersCount;

            if (LcrGame.Games.Any())
                LcrGame.Games.Clear();

            await Task.Run(() => PlayGame());

            MinTurns = LcrGame.Games.Min(o => o.TurnsCount);
            MaxTurns = LcrGame.Games.Max(o => o.TurnsCount);
            AvgTurns = Math.Round(LcrGame.Games.Average(o => o.TurnsCount), 1);
        }

        private void PlayGame()
        {
            for (int i = 0; i < GamesCount; i++)
            {
                var game = new Game(PlayersCount, LcrGame.InitialChipsCount);
                LcrGame.Games.Add(game);
            }

            LcrGame.Games.ForEach(o => o.PlayGame());
        }
        #endregion

        #region Validation
        private const int V = default;

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "PlayersCount":
                        result = PlayersCountValidation();
                        break;

                    case "GamesCount":
                        result = GamesCountValidation();
                        break;

                    default:
                        break;
                }

                return result;
            }
        }

        public void Reset()
        {
            PlayersCount = V;
            GamesCount = V;
        }

        public bool IsValid()
        {
            var playersCountValid = PlayersCountValidation();
            var gamesCountValid = GamesCountValidation();

            var result = playersCountValid == null && gamesCountValid == null;
            return result;
        }

        private string GamesCountValidation()
        {
            string result = null;
            var mgcnt = LcrGame.MinGamesCount;

            if (GamesCount < mgcnt)
                result = string.Format("Please enter a valid GamesCount value at least {0}", mgcnt);

            return result;
        }

        private string PlayersCountValidation()
        {
            string result = null;
            var mpcnt = LcrGame.MinPlayersCount;

            if (PlayersCount < mpcnt)
                result = string.Format("Please enter a valid PlayersCount value at least {0}", mpcnt);

            return result;
        }

        #endregion
    }
}