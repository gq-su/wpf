using LcrSimulator.Commands;
using LcrSimulator.Model;
using Prism.Mvvm;
using System;
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

            PropertyChanged += MainWindowViewModel_PropertyChanged;
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
                RaisePropertyChanged();
            }
        }

        private int? _gamesCount;
        public int? GamesCount
        {
            get { return _gamesCount; }
            set
            {
                if (_gamesCount == value)
                    return;

                _gamesCount = value;
                RaisePropertyChanged();
            }
        }

        private int _minTurns;
        public int MinTurns
        {
            get { return _minTurns; }
            set
            {
                if (_minTurns == value)
                    return;

                _minTurns = value;
                RaisePropertyChanged();
            }
        }

        private int _maxTurns;
        public int MaxTurns
        {
            get { return _maxTurns; }
            set
            {
                if (_maxTurns == value)
                    return;

                _maxTurns = value;
                RaisePropertyChanged();
            }
        }

        private int _avgTurns;
        public int AvgTurns
        {
            get { return _avgTurns; }
            set
            {
                if (_avgTurns == value)
                    return;

                _avgTurns = value;
                RaisePropertyChanged();
            }
        }

        private void MainWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PlayersCount" || e.PropertyName == "GamesCount")
                CanExecutePlayGame();
        }


        #region PlayGameCommand
        public ICommand PlayGameCommand { get; private set; }

        private bool CanExecutePlayGame()
        {
            //Actual project shoulld use message service to validate 
            //and popup message if failed to meet minmum PlayersCount or GamesCount
            return PlayersCount > 2 && GamesCount > 2;
        }

        private void PlayGameCommandHandler()
        {
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