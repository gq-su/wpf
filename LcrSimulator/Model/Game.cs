using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LcrSimulator.Model
{
    public class Game : BindableBase
    {
        private readonly List<Player> _players;

        public Game()
        {
            _players = new List<Player>();
        }

        public Game(int playersCount) : this()
        {
            for (int i = 0; i < playersCount; i++)
            {
                var player = new Player
                {
                    Index = i,
                };

                player.PropertyChanged += Player_PropertyChanged;

                _players.Add(player);
            }
        }

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Played")
            {
                CheckGameResult((Player)sender);
            }
        }

        public int Index { get; set; }

        public int TurnsCount { get; set; } = 0;

        public bool GameOver { get; set; }

        public void PlayGame()
        {
            do
            {
                _players.ForEach((player) =>
                {
                    if (!GameOver)
                        player.Play(Index + TurnsCount);
                });

            } while (!GameOver);
        }

        private void CheckGameResult(Player currPlayer)
        {
            TurnsCount += 1;
            var dices = currPlayer.Dices.FindAll(o => o.IsActive);

            if (dices.Any(o => o.DiceFace == 'L'))
            {
                var prevPlayerIndex = currPlayer.Index - 1;
                prevPlayerIndex = (prevPlayerIndex + _players.Count) % _players.Count;
                var prevPlayer = _players.ElementAt(prevPlayerIndex);

                var lCount = dices.FindAll(o => o.DiceFace == 'L').Count;
                currPlayer.ChipsCount -= lCount;
                prevPlayer.ChipsCount += lCount;
            }

            if (dices.Any(o => o.DiceFace == 'C'))
            {
                var cCount = dices.FindAll(o => o.DiceFace == 'C').Count;
                currPlayer.ChipsCount -= cCount;
            }

            if (dices.Any(o => o.DiceFace == 'R'))
            {
                var nextPlayerIndex = currPlayer.Index + 1;
                nextPlayerIndex = nextPlayerIndex % _players.Count;
                var nextPlayer = _players.ElementAt(nextPlayerIndex);

                var rCount = dices.FindAll(o => o.DiceFace == 'R').Count;
                currPlayer.ChipsCount -= rCount;
                nextPlayer.ChipsCount += rCount;
            }

            GameOver = _players.FindAll(o => o.ChipsCount > 0).Count == 1;
        }
    }
}
