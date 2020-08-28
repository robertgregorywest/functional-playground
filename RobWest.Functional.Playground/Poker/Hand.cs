using System.Collections.Generic;
using static RobWest.Functional.F;

namespace RobWest.Functional.Playground.Poker
{
    public class Hand : List<Card>
    {
        public static Validation<Hand> CreateHand(IEnumerable<string> inputs) => Valid(new Hand());
        
        // private Hand()
        // { }

        private static bool IsValid(IEnumerable<string> inputs)
        {
            return true;
        }
    }
}