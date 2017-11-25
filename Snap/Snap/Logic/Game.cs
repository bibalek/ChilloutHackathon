using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSSocketClient
{
    public class Game
    {
        public Game()
        {
            cards = new List<Card>();
            croupier = new Croupier();
            players = new List<Player>();
            players.Add(new Player());
            players.Add(new Player());
        }

        public List<Player> players;
        public Croupier croupier;
        public List<Card> cards { get; set; }
    }
}
