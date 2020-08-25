using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static RobWest.Functional.F;

[assembly: InternalsVisibleTo("Robwest.Functional.Tests")]

namespace RobWest.Functional.Playground
{
    // Poker hand ranking challenge: https://edabit.com/challenge/MnPogX5KgggaRpaJo
    public struct Card
    {
        public int Value { get; }
        public Suit Suit { get; }

        public static Option<Card> Create(string input) => TryParseCard(input, out var card) ? Some(card) : None;

        private Card(int value, Suit suit)
        {
            if (!IsValidValue(value))
            {
                throw new ArgumentException($"{value.ToString()} is not within the correct range");
            }
            
            (Value, Suit) = (value, suit);
        }

        private static Card Default => new Card(14, Suit.Spades);

        internal static bool IsValid(string value)
        {
            return new Regex(@"^(10|[2-9,J,Q,K,A]{1})[c,d,h,s]{1}$").IsMatch(value);
        }

        private static bool IsValidValue(int value) => value > 1 && value < 15;

        internal static bool TryParseCard(string input, out Card card)
        {
            card = Default;
            
            if (input.Length < 2 || input.Length > 3) return false;

            try
            {
                var suit = input[^1] switch
                {
                    'c' => Suit.Clubs,
                    'd' => Suit.Diamonds,
                    'h' => Suit.Hearts,
                    's' => Suit.Spades,
                    _ => throw new ArgumentException()
                };
            
                int value;
                var valueInput = input[..^1];

                if (int.TryParse(valueInput, out var result))
                {
                    if (IsValidValue(result))
                    {
                        value = result;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    // We have a face card: J,Q,K,A which we represent with 11,12,13,14
                    value = valueInput switch
                    {
                        "J" => 11,
                        "Q" => 12,
                        "K" => 13,
                        "A" => 14,
                        _ => throw new ArgumentException()
                    };
                }
                card = new Card(value, suit);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }

    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public static class Poker
    {
        
    }
}