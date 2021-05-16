using System.Collections.Generic;

namespace LcrSimulator.Model
{
    public class LcrGame
    {
        public LcrGame()
        {
        }

        public const int MinPlayersCount = 3;

        public const int MinGamesCount = 3;

        public int PlayersCount { get; set; }

        public int GamesCount { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();

        public List<Game> Games { get; set; } = new List<Game>();
    }
}
