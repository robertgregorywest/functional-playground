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

            private CardNumber(int value)
            {
                Value = value;
            }

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
        public readonly struct HandResult
        {
            public bool IsMatch { get; }
            public string Name { get; }
            public int Rank { get; }

            private HandResult(bool isMatch, string name, int rank) => (IsMatch, Name, Rank) = (isMatch, name, rank);

            public static HandResult Match(string name, int rank) => new HandResult(true, name, rank);

            public static HandResult NoMatch(string name) => new HandResult(false, name, 0);
        }

        private static readonly IList<Func<IList<Card>, HandResult>> HandMatchers =
            new List<Func<IList<Card>, HandResult>>
            {
                IsRoyalFlush,
                IsStraightFlush,
                IsFourOfAKind,
                IsHighCard
            };

        public static string PokerHandRanking(IEnumerable<string> cardInputs)
        {
            var cards = new List<Card>();

            // IEnumerable<Validation<Card>> -> Validation<IEnumerable<Card>>
            // Valid if 5 valid cards

            cardInputs
                .Take(5)
                .Map(Card.CreateValidCard)
                .ForEach(v => v.Match(
                    _ => { },
                    card => cards.Add(card)
                ));

            foreach (var handMatcher in HandMatchers)
            {
                var result = handMatcher(cards);
                if (result.IsMatch) return result.Name;
            }

            return "No matching hand";
        }

        private static IEnumerable<int> CardValues(this IEnumerable<Card> cards)
            => cards.Select(c => (int) c.Value);

        private static int SuitCount(this IEnumerable<Card> cards)
            => cards.GroupBy(c => c.Suit).Count();
        
        private static bool AreConsecutive(this IEnumerable<Card> cards)
            => !cards.CardValues().Select((i,j) => i-j).Distinct().Skip(1).Any();

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

        public const string RoyalFlush = "Royal Flush";
        public const string StraightFlush = "Straight Flush";
        public const string FourOfAKind = "Four of a Kind";


        public const string HighCard = "High Card";

        private static HandResult IsRoyalFlush(IList<Card> cards)
        {
            var royalFlush = new[] {10, 11, 12, 13, 14};

            return cards.SuitCount() == 1 && cards.CardValues().MatchExactly(royalFlush)
                ? HandResult.Match(RoyalFlush, 1)
                : HandResult.NoMatch(RoyalFlush);
        }

        private static HandResult IsStraightFlush(IList<Card> cards)
        {
            return cards.SuitCount() == 1 && cards.AreConsecutive()
                ? HandResult.Match(StraightFlush, 2)
                : HandResult.NoMatch(StraightFlush);
        }

        private static HandResult IsFourOfAKind(IList<Card> cards)
        {
            var cardValues = cards.CardValues().GroupBy(v => v).Count();
            return cardValues <= 2 ? HandResult.Match(FourOfAKind, 3) : HandResult.NoMatch(FourOfAKind);
        }

        private static HandResult IsHighCard(IList<Card> cards) => HandResult.Match(HighCard, 100);
    }
}