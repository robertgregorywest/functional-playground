using System;
using System.ComponentModel.DataAnnotations;
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
            public static readonly Func<string, Option<CardNumber>> Create = s =>
            {
                if (int.TryParse(s, out var value))
                {
                    return (value > 1 && value < 15) ? Some(new CardNumber(value)) : None;
                }

                // We have a face card: J,Q,K,A which we represent with 11,12,13,14
                return s.ToUpperInvariant() switch
                {
                    "J" => Some(new CardNumber(11)),
                    "Q" => Some(new CardNumber(12)),
                    "K" => Some(new CardNumber(13)),
                    "A" => Some(new CardNumber(14)),
                    _ => None
                };
            };

            private int Value { get; }

            private CardNumber(int value) { Value = value; }
            
            public static implicit operator int(CardNumber c) => c.Value;
            
            public static implicit operator CardNumber(int i) => new CardNumber(i);
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

        private Card(CardNumber value, SuitType suit)
        {
            (Value, Suit) = (value, suit);
        }

        private static readonly Func<string, Validation<(string value, string suit)>> ValidInput = s =>
        {
            if (s.Length < 2 || s.Length > 3) return Invalid($"{s} is not a valid input");
            return (s[..^1], s[^1].ToString());
        };
        
        private static readonly Func<string, Validation<CardNumber>> ValidCardNumber = s
            => CardNumber.Create(s).Match(
                () => Error($"{s} is not a valid number"),
                n => Valid(n));
        
        private static readonly Func<string, Validation<SuitType>> ValidSuitType = s 
            => s.ToLowerInvariant() switch
            {
                "c" => Valid(SuitType.Clubs),
                "d" => Valid(SuitType.Diamonds),
                "h" => Valid(SuitType.Hearts),
                "s" => Valid(SuitType.Spades),
                _ => Invalid($"{s} is not a valid suit character")
            };
    }

    public static class Poker
    {
        
    }
}