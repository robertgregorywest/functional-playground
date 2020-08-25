using NUnit.Framework;
using RobWest.Functional.Playground;
using static RobWest.Functional.F;

namespace RobWest.Functional.Tests
{
    public class PokerTests
    {
        [TestCase("Qs", ExpectedResult=true)]
        [TestCase("3c", ExpectedResult=true)]
        [TestCase("10s", ExpectedResult=true)]
        [TestCase("Jd", ExpectedResult=true)]
        [TestCase("3C", ExpectedResult=false)]
        [TestCase("15d", ExpectedResult=false)]
        public bool TryParseCard(string value)
        {
            return Card.TryParseCard(value, out var card);
        }
        
        [TestCase("Ah", ExpectedResult=true)]
        public bool CreateCard(string value)
        {
            return !Card.Create(value).Equals(None);
        }
    }
}