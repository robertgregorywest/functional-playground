using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static RobWest.Functional.F;

[assembly: InternalsVisibleTo("Robwest.Functional.Tests")]

namespace RobWest.Functional.Playground
{
    // Poker hand ranking challenge: https://edabit.com/challenge/MnPogX5KgggaRpaJo
    public readonly struct Card
    {
        public enum SuitType
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }
        
        public SuitType Suit { get; }

        public readonly struct CardNumber
        {
            private int Value { get; }
            
            private static readonly IDictionary<int, string> FaceCards = new Dictionary<int, string>
            {
                {11, "J"},
                {12, "Q"},
                {13, "K"},
                {14, "A"}
            };

            private static readonly IDictionary<string, int> FaceCardsReversed =
                FaceCards.ToDictionary(pair => pair.Value, pair => pair.Key);
            
            public static readonly Func<string, Option<CardNumber>> Create = s =>
            {
                if (int.TryParse(s, out var value))
                {
                    return (value > 1 && value < 15) ? Some(new CardNumber(value)) : None;
                }
                return FaceCardsReversed.ContainsKey(s) ? Some(new CardNumber(FaceCardsReversed[s])) : None;
            };
            
            private CardNumber(int value) { Value = value; }
            
            public static implicit operator int(CardNumber c) => c.Value;
            
            public static implicit operator CardNumber(int i) => new CardNumber(i);

            public override string ToString()
            {
                return Value < 11 ? Value.ToString() : FaceCards[Value];
            }
        }
        
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

    public static class Poker
    {
        private static readonly IList<Func<IEnumerable<Card>, Validation<string>>> combinations =
            new List<Func<IEnumerable<Card>, Validation<string>>>
        {
            IsRoyalFlush
        };

        public static string PokerHandRanking(IEnumerable<string> cardInputs)
        {
            var cards = cardInputs
                .Take(5)
                .Map(Card.CreateValidCard);




            return "Royal Flush";
        }

        internal static Validation<string> IsRoyalFlush(IEnumerable<Card> cards)
        {
            var royalFlush = Enumerable.Range(10, 5);
            var cardValues = cards.Select(c => (int)c.Value).ToList();

            return cards.GroupBy(c => c.Suit).Count() == 1 && !royalFlush.Except(cardValues).Any() 
                ? Valid("Royal Flush") 
                : Invalid();
        }
        
        
    }
}