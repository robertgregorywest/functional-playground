using System;
using static RobWest.Functional.F;

namespace RobWest.Functional.Playground.Poker
{
    public readonly struct Card
    {
        public SuitType Suit { get; }
        public CardNumber Value { get; }

        public static Validation<Card> CreateValidCard(string s) =>
            ValidInput(s).Match(
                (e) => Invalid(e),
                tuple => CreateValidCard(tuple.value, tuple.suit));

        private static Validation<Card> CreateValidCard(string value, string suit)
            => Valid(Create)
                .Apply(ValidCardNumber(value))
                .Apply(ValidSuitType(suit));

        private static readonly Func<CardNumber, SuitType, Card> Create = (value, type) => new Card(value, type);

        private Card(CardNumber value, SuitType suit) => (Value, Suit) = (value, suit);

        private static readonly Func<string, Validation<(string value, string suit)>> ValidInput = s =>
        {
            if (s.Length < 2 || s.Length > 3) return Invalid($"{s} is not a valid input");
            return (s[..^1].ToUpperInvariant(), s[^1].ToString().ToLowerInvariant());
        };

        private static readonly Func<string, Validation<CardNumber>> ValidCardNumber = s
            => CardNumber.Create(s).Match(
                () => Error($"{s} is not a valid card number"),
                n => Valid(n));

        private static readonly Func<string, Validation<SuitType>> ValidSuitType = s
            => s switch
            {
                "c" => Valid(SuitType.Clubs),
                "d" => Valid(SuitType.Diamonds),
                "h" => Valid(SuitType.Hearts),
                "s" => Valid(SuitType.Spades),
                _ => Invalid($"{s} is not a valid suit character")
            };

        public override string ToString() => $"{Value.ToString()}{Suit.ToString()}";
    }
}