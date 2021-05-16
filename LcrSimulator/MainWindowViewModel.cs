using LcrSimulator.Commands;
using LcrSimulator.Model;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace LcrSimulator
{
    internal class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(LcrGame lcrGame)
        {
            LcrGame = lcrGame;
            PlayGameCommand = new RelayCommand(param => PlayGameCommandHandler(), param => CanExecutePlayGame());
        }

        public LcrGame LcrGame { get; set; }

        private int? _playersCount;
        public int? PlayersCount
        {
            get { return _playersCount; }
            set
            {
                if (_playersCount == value)
                    return;

                _playersCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersCount"));
            }
        }

        private int? _gamesCount;
        public int? GamesCount
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
            set
            {
                SetProperty(ref _minTurns, value);
                RaisePropertyChanged();
            }
        }

        private int _maxTurns;
        public int MaxTurns
        {
            get { return _maxTurns; }
            set
            {
                SetProperty(ref _maxTurns, value);
                RaisePropertyChanged();
            }
        }

        private int _avgTurns;
        public int AvgTurns
        {
            get { return _avgTurns; }
            set
            {
                SetProperty(ref _avgTurns, value);
                RaisePropertyChanged();
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName) {
                case "PlayersCount":
                case "GamesCount":
                    CanExecutePlayGame();
                    break;
                default:
                    break;
            }
        }

        #region PlayGameCommand
        public ICommand PlayGameCommand { get; private set; }

        private bool CanExecutePlayGame()
        {
            //Actual project shoulld implement IDataErrorInfo to validate 
            //and popup tooltip if failed to meet minmum value
            return PlayersCount > 2 && GamesCount > 2;
        }

        private void PlayGameCommandHandler()
        {
            MinTurns = 0;
            LcrGame.GamesCount = GamesCount.GetValueOrDefault();

            LcrGame.PlayersCount = PlayersCount.GetValueOrDefault();

            if (LcrGame.Games.Any())
                LcrGame.Games.Clear();

            for (int i = 0; i < LcrGame.GamesCount; i++)
            {
                var game = new Game(LcrGame.PlayersCount);
                game.PlayGame();
                LcrGame.Games.Add(game);
            }

            MinTurns = LcrGame.Games.Min(o => o.TurnsCount);
            MaxTurns = LcrGame.Games.Max(o => o.TurnsCount);
            AvgTurns = (int)Math.Round(LcrGame.Games.Average(o => o.TurnsCount), 0);
        }
        #endregion
    }
}