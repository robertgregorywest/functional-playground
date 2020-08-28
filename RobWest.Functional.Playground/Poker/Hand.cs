using System.Collections.Generic;
using System.Linq;
using static RobWest.Functional.F;

namespace RobWest.Functional.Playground.Poker
{
    public class Hand : List<Card>
    {
        internal const string RoyalFlush = "Royal Flush";
        internal const string StraightFlush = "Straight Flush";
        internal const string FourOfAKind = "Four of a Kind";
        internal const string FullHouse = "Full House";
        internal const string Flush = "Flush";
        internal const string Straight = "Straight";
        internal const string ThreeOfAKind = "Three of a Kind";
        internal const string TwoPair = "Two Pair";
        internal const string Pair = "Pair";
        internal const string HighCard = "High Card";
        internal const string FiveCards = "Hand must contain five cards";
        internal const string Duplicates = "Hand cannot contain duplicates";
        
        public static Validation<Hand> Create(IEnumerable<string> inputs)
        {
            var hand = new Hand();
            var cardErrors = new List<Error>();

            inputs
                .Map(Card.CreateValidCard)
                .ForEach(v => v.Match(
                    errors => cardErrors.AddRange(errors),
                    card => hand.Add(card)
                ));

            if (cardErrors.Any()) return Invalid(cardErrors);

            var validators = new List<Validator<Hand>> {MustHaveFiveCards, CannotHaveDuplicates};

            return HarvestErrors(validators)(hand).Match(
                errors => Invalid(errors),
                Valid);
        }

        private Hand()
        {
            // Private to ensure creation runs through validation
        }
        
        private delegate Validation<T> Validator<T>(T t);

        private static readonly Validator<Hand> MustHaveFiveCards =
            hand => hand.Count() == 5 ? Valid(hand) : Invalid(FiveCards);

        private static readonly Validator<Hand> CannotHaveDuplicates =
            hand => hand.Count() == hand.Distinct().Count() ? Valid(hand) : Invalid(Duplicates);

        private static Validator<T> HarvestErrors<T>(IEnumerable<Validator<T>> validators)
            => t =>
            {
                var errors = validators
                    .Map(validate => validate(t))
                    .Bind(v => v.Match(
                        Invalid: errs => Some(errs),
                        Valid: _ => None))
                    .ToList();

                return errors.Count == 0
                    ? Valid(t)
                    : Invalid(errors.Flatten());
            };
    }
}