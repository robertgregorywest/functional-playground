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
            var validators = new List<Validator<Hand>> {MustHaveFiveCards, CannotHaveDuplicates};
            
            return HarvestErrors(validators)(inputs
                .Map(Card.CreateValidCard)
                .Bind(v => v.Match(
                    _ => None,
                    card => Some(card))) as Hand)
                .Match(
                errors => Invalid(errors),
                hand => Valid(hand));
        }

        private Hand()
        {
            // Private to ensure creation runs through validation
        }

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