using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LcrSimulator.Model
{
    public class Game : BindableBase
    {
        private List<Player> _players;

        public Game(int playersCount)
        {
            _players = new List<Player>();
            for (int i = 0; i < playersCount; i++)
            {
                var player = new Player { Index = i };
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

        //takes how many rounds to win
        public int TurnsCount { get; set; } = 0;

        public bool GameOver { get; set; }

        public void PlayGame()
        {
            do
            {
                _players.ForEach((player)=>player.Play());

            } while (!GameOver);
        }

        private void CheckGameResult(Player currPlayer)
        {
            TurnsCount += 1;
            var dices = currPlayer.Dices;

            if (dices.Any(dice => dice.DiceFace == "D"))
                return;

            if (dices.Any(o => o.DiceFace == "L"))
            {
                var prevPlayerIndex = currPlayer.Index - 1;
                prevPlayerIndex = (prevPlayerIndex + _players.Count) % _players.Count;
                var prevPlayer = _players.ElementAt(prevPlayerIndex);

                var lCount = dices.FindAll(o => o.DiceFace == "L").Count;
                prevPlayer.ChipsCount += lCount;
            }

            if (dices.Any(o => o.DiceFace == "R"))
            {
                var nextPlayerIndex = currPlayer.Index + 1;
                nextPlayerIndex = nextPlayerIndex % _players.Count;
                var nextPlayer = _players.ElementAt(nextPlayerIndex);

                var lCount = dices.FindAll(o => o.DiceFace == "R").Count;
                nextPlayer.ChipsCount += lCount;
            }

            currPlayer.ChipsCount -= dices.FindAll(o => o.DiceFace != "D").Count;

            GameOver = _players.FindAll(o => o.ChipsCount > 0).Count == 1;
        }
    }
}
