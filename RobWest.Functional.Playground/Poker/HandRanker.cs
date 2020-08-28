using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static RobWest.Functional.Playground.Poker.HandMatcherResult;

[assembly: InternalsVisibleTo("Robwest.Functional.Tests")]

namespace RobWest.Functional.Playground.Poker
{
    /// <summary>
    /// Solution for Poker hand ranking challenge: https://edabit.com/challenge/MnPogX5KgggaRpaJo
    /// </summary>
    public static class HandRanker
    {
        private static readonly IList<Func<Hand, HandMatcherResult>> HandMatchers =
            new List<Func<Hand, HandMatcherResult>>
            {
                IsRoyalFlush,
                IsStraightFlush,
                IsFourOfAKind,
                IsFullHouse,
                IsFlush,
                IsStraight,
                IsThreeOfAKind,
                IsTwoPair,
                IsPair
            };

        public static HandRankerResult HandRanking(IEnumerable<string> cardInputs)
        {
            var cards = new Hand();

            // IEnumerable<Validation<Card>> -> Validation<Hand>
            // Valid if 5 valid cards
            // Cannot contain duplicates

            cardInputs
                .Take(5)
                .Map(Card.CreateValidCard)
                .ForEach(v => v.Match(
                    _ => { },
                    card => cards.Add(card)
                ));

            var rank = 1;
            foreach (var handMatcher in HandMatchers)
            {
                var result = handMatcher(cards);
                if (result.IsMatch) return new HandRankerResult(result.Name, rank);
                rank++;
            }

            return new HandRankerResult(HighCard, 0);
        }
        
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

        private static HandMatcherResult IsRoyalFlush(Hand cards)
        {
            var royalFlush = new[] {10, 11, 12, 13, 14};

            return cards.SuitCount() == 1 && cards.CardValues().MatchExactly(royalFlush)
                ? Match(RoyalFlush)
                : NoMatch();
        }

        private static HandMatcherResult IsStraightFlush(Hand cards)
        {
            return cards.SuitCount() == 1 && cards.AreConsecutive()
                ? Match(StraightFlush)
                : NoMatch();
        }

        private static HandMatcherResult IsFourOfAKind(Hand cards)
        {
            return cards.HasOfAKind(4)
                ? Match(FourOfAKind) 
                : NoMatch();
        }

        private static HandMatcherResult IsFullHouse(Hand cards)
            => cards.CardValues().GroupBy(v => v).Count() == 2
                ? Match(FullHouse)
                : NoMatch(); 
        
        private static HandMatcherResult IsFlush(Hand cards)
        {
            return cards.SuitCount() == 1
                ? Match(Flush)
                : NoMatch();
        }
        
        private static HandMatcherResult IsStraight(Hand cards)
        {
            return cards.AreConsecutive()
                ? Match(Straight)
                : NoMatch();
        }
        
        private static HandMatcherResult IsThreeOfAKind(Hand cards)
        {
            return cards.HasOfAKind(3)
                ? Match(ThreeOfAKind) 
                : NoMatch();
        }
        
        private static HandMatcherResult IsTwoPair(Hand cards)
            => cards.CardValues().GroupBy(v => v).Count() == 3
                ? Match(TwoPair)
                : NoMatch(); 
        
        private static HandMatcherResult IsPair(Hand cards)
        {
            return cards.HasOfAKind(2)
                ? Match(Pair) 
                : NoMatch();
        }
    }
}