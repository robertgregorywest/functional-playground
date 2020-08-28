using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Robwest.Functional.Tests")]

namespace RobWest.Functional.Playground.Poker
{
    /// <summary>
    /// Solution for Poker hand ranking challenge: https://edabit.com/challenge/MnPogX5KgggaRpaJo
    /// </summary>
    public static class PokerHandRanker
    {
        private static readonly IList<Func<IList<Card>, HandResult>> HandRankers =
            new List<Func<IList<Card>, HandResult>>
            {
                IsRoyalFlush,
                IsStraightFlush,
                IsFourOfAKind,
                IsHighCard
            };

        public static string HandRanking(IEnumerable<string> cardInputs)
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

            foreach (var handRanker in HandRankers)
            {
                var result = handRanker(cards);
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
        public const string FullHouse = "Full House";
        public const string Flush = "Flush";
        public const string Straight = "Straight";
        public const string ThreeOfAKind = "Three of a Kind";
        public const string TwoPair = "Two Pair";
        public const string Pair = "Pair";
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