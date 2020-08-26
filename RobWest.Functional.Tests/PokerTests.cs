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
    }
}