using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSSocketClient
{
    
    public class Player
    {
        public Player()
        {
            Hand = new List<Card>();
        }

        public bool IsSB { get; set; }
        public bool IsBB { get; set; }
        public bool IsDealer { get; set; }
        public int AccountBalance { get; set; }
        public List<Card> Hand { get; set; }
        //public List<Card> Hand { get; set; }
    }





    public class Deal
    {
        
    }
}
