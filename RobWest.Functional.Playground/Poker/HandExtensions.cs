using System.Collections.Generic;
using System.Linq;

namespace RobWest.Functional.Playground.Poker
{
    public static class HandExtensions
    {
        internal static IEnumerable<int> CardValues(this Hand cards)
            => cards.Select(c => (int) c.Value);

        internal static int SuitCount(this Hand cards)
            => cards.GroupBy(c => c.Suit).Count();
        
        internal static bool AreConsecutive(this Hand cards)
            => !cards.CardValues().OrderBy(v => v).Select((i,j) => i-j).Distinct().Skip(1).Any();

        internal static bool HasOfAKind(this Hand cards, int count)
            => cards.CardValues().GroupBy(v => v).Any(g => g.Count() == count);
    }
}