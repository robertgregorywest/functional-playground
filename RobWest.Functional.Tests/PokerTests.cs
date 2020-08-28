using NUnit.Framework;
using RobWest.Functional.Playground;

namespace RobWest.Functional.Tests
{
    public class PokerTests
    {
        [TestCase("Qs", ExpectedResult=true)]
        [TestCase("3c", ExpectedResult=true)]
        [TestCase("10s", ExpectedResult=true)]
        [TestCase("Jd", ExpectedResult=true)]
        [TestCase("3C", ExpectedResult=true)]
        [TestCase("15d", ExpectedResult=false)]
        public bool CreateValidCard(string value)
        {
            return Card.CreateValidCard(value).IsValid;
        }
        
        // Royal Flush: A, K, Q, J, 10, all with the same suit.
        // Straight Flush: Five cards in sequence, all with the same suit.
        // Four of a Kind: Four cards of the same rank.
        // Full House: Three of a Kind with a Pair.
        // Flush: Any five cards of the same suit, not in sequence.
        // Straight: Five cards in a sequence, but not of the same suit.
        // Three of a Kind: Three cards of the same rank.
        // Two Pair: Two different Pair.
        // Pair: Two cards of the same rank.
        // High Card: No other valid combination.
        
        [TestCase("10h", "Jh", "Qh", "Ah", "Kh", ExpectedResult=Poker.RoyalFlush)]
        [TestCase("2h", "3h", "4h", "5h", "6h", ExpectedResult=Poker.StraightFlush)]
        [TestCase("3h", "5h", "Qs", "9h", "Ad", ExpectedResult=Poker.HighCard)]
        [TestCase("10s", "10c", "8d", "10d", "10h", ExpectedResult=Poker.FourOfAKind)]
        public string PokerHandRanking(params string[] cards)
        {
            var result = Poker.PokerHandRanking(cards);
            return result;
        }
    }
}