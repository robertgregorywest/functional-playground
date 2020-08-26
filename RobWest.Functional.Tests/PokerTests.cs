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
        
        [TestCase("10h", "Jh", "Qh", "Ah", "Kh", ExpectedResult="Royal Flush")]
        [TestCase("3h", "5h", "Qs", "9h", "Ad", ExpectedResult="High Card")]
        [TestCase("10s", "10c", "8d", "10d", "10h", ExpectedResult="Four of a Kind")]
        public string PokerHandRanking(params string[] cards)
        {
            return Poker.PokerHandRanking(cards);
        }
    }
}