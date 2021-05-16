﻿using LcrSimulator.Commands;
using LcrSimulator.Model;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace LcrSimulator
{
    internal class MainWindowViewModel : BindableBase, IDataErrorInfo
    {
        public MainWindowViewModel(LcrGame lcrGame)
        {
            LcrGame = lcrGame;
            PlayGameCommand = new RelayCommand(param => PlayGameCommandHandler(), param => CanExecutePlayGame());
        }

        public LcrGame LcrGame { get; set; }

        private int _playersCount = 3;
        public int PlayersCount
        {
            get { return _playersCount; }
            set
            {
                SetProperty(ref _playersCount, value);
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersCount"));
            }
        }

        private int _gamesCount = 3;
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
            LcrGame.GamesCount = GamesCount;
            LcrGame.PlayersCount = PlayersCount;

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

            if (GamesCount < 3)
                result = "Please enter a valid GamesCount value at least 3";

            return result;
        }

        private string PlayersCountValidation()
        {
            string result = null;

            if (PlayersCount < 3)
                result = "Please enter a valid PlayersCount value at least 3";

            return result;
        }

        #endregion
    }
}