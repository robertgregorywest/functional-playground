using System;
using System.Collections.Generic;
using System.Linq;
using static RobWest.Functional.F;

namespace RobWest.Functional.Playground.Poker
{
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
}