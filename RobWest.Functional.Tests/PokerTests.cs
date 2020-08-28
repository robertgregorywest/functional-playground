using System;
using NUnit.Framework;
using RobWest.Functional.Playground.Poker;
using static RobWest.Functional.Playground.Poker.HandRanker;

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
        [TestCase("Bd", ExpectedResult=false)]
        [TestCase("7y", ExpectedResult=false)]
        public bool CreateValidCardTest(string value)
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
        
        [TestCase("10h", "Jh", "Qh", "Ah", "Kh", ExpectedResult=RoyalFlush+"1")]
        [TestCase("2h", "3h", "4h", "5h", "6h", ExpectedResult=StraightFlush+"2")]
        [TestCase("10s", "10c", "8d", "10d", "10h", ExpectedResult=FourOfAKind+"3")]
        [TestCase("10s", "10c", "8d", "10d", "8h", ExpectedResult=FullHouse+"4")]
        [TestCase("10s", "2s", "8s", "6s", "8s", ExpectedResult=Flush+"5")]
        [TestCase("2s", "3h", "4d", "6s", "5c", ExpectedResult=Straight+"6")]
        [TestCase("10s", "10c", "8d", "10d", "Jc", ExpectedResult=ThreeOfAKind+"7")]
        [TestCase("10s", "10c", "8d", "8c", "Jc", ExpectedResult=TwoPair+"8")]
        [TestCase("10s", "10c", "8d", "Qh", "Jc", ExpectedResult=Pair+"9")]
        [TestCase("3h", "5h", "Qs", "9h", "Ad", ExpectedResult=HighCard+"0")]
        public string HandRankingTest(params string[] cards)
        {
            return HandRanking(cards).ToString();
        }
    }
}