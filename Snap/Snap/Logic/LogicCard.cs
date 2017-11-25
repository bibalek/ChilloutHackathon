using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSSocketClient
{
    public enum Figures { Two = 2 , Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }
    public enum Colors { C, S, D, H }

    public class Card
    {
        public Card() { }

        public Card(Figures figure, Colors color)
        {
            this.figure = figure;
            this.color = color;
        }

        public Colors color { get; set; }
        public Figures figure { get; set; }
    }

}
