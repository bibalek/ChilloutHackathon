using SnapCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fiveCardEvaluator = new Evaluator();
            //fiveCardEvaluator.SaveToFile("./eval_tables/five_card.ser");

            var fiveCardEvaluator = new Evaluator(fileName: "./eval_tables/five_card.ser");

            Card card1 = new Card("2S");
            
            Card card2 = new Card("2D");
            Card card3 = new Card("3C");
            Card card4 = new Card("4C");
            Card card5 = new Card("5C");


            Card card10 = new Card("3S");
            Card card6 = new Card("3D");
            Card card7 = new Card("4C");
            Card card8 = new Card("5C");
            Card card9 = new Card("6C");
            int val = card1.GetHashCode(card1);
            int val2 = card2.GetHashCode(card2);
            int val3 = card3.GetHashCode(card3);
            int val4 = card4.GetHashCode(card4);
            int val5 = card5.GetHashCode(card5);

            Hand handA = new Hand();
            Hand handB = new Hand();

            handA.Cards.Add(card1);
            handA.Cards.Add(card2);
            handA.Cards.Add(card3);
            handA.Cards.Add(card4);
            handA.Cards.Add(card5);

            handB.Cards.Add(card10);
            handB.Cards.Add(card6);
            handB.Cards.Add(card7);
            handB.Cards.Add(card8);
            handB.Cards.Add(card9);



            int res = handA.GetStrength().CompareTo(handB.GetStrength());
            if(res == 1)
            {
                Console.WriteLine("Wygral gracz: handA");
            }
            else if( res == -1 )
            {
                Console.WriteLine("Wygral gracz: handB");
                Console.WriteLine(handB.GetStrength().HandRanking);
                foreach(int kicker in handB.GetStrength().Kickers)
                {
                    Console.WriteLine(kicker);
                }
                
                handB.PrintColoredCards("\t");
                Console.WriteLine(handB.ToString());
                Console.WriteLine(handB.GetHashCode());
            }
            else
            {
                Console.WriteLine("remis");
            }
            


            Console.Read();
            //fiveCardEvaluator.Evaluate();

            //var sixCardEvaluator = new Evaluator(sixCard: true);
            //sixCardEvaluator.SaveToFile("./eval_tables/six_card.ser");

            //var sevenCardEvaluator = new Evaluator(sixCard: true, sevenCard: true);
            //sevenCardEvaluator.SaveToFile("./eval_tables/seven_card.ser");
        }
    }
}
