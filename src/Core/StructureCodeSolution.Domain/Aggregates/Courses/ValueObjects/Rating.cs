using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Aggregates.Courses.Exceptions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects
{
    public sealed class Rating : ValueObject
    {
        public decimal StarRating { get; private set; }
        public int NumberOfRatings { get; private set; }

        private Rating() { }

        private Rating(decimal starRating, int numberOfRatings)
        {
            StarRating = starRating;
            NumberOfRatings = numberOfRatings;
        }

        public static Rating Create() => new Rating(0, 0);

        public static Rating Create(decimal starRating, int numberOfRatings)
        {
            if (starRating < 0 || starRating > 5)
                throw new RatingException.InvalidRatingException();

            if (numberOfRatings < 0)
                throw new ArgumentException("Number of ratings cannot be negative", nameof(numberOfRatings));

            return new Rating(starRating, numberOfRatings);
        }

        public void AddRating(decimal newRating)
        {
            if (newRating < 0 || newRating > 5)
                throw new RatingException.InvalidRatingException();

            var totalStars = (StarRating * NumberOfRatings) + newRating;
            NumberOfRatings++;
            StarRating = Math.Round(totalStars / NumberOfRatings, 2);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StarRating;
            yield return NumberOfRatings;
        }
    }
}
