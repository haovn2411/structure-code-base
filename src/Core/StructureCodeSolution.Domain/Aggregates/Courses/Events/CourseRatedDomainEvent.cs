using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class CourseRatedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public decimal NewRating { get; }
        public decimal AverageRating { get; }
        public int TotalRatings { get; }

        public CourseRatedDomainEvent(Guid courseId, decimal newRating, decimal averageRating, int totalRatings)
        {
            CourseId = courseId;
            NewRating = newRating;
            AverageRating = averageRating;
            TotalRatings = totalRatings;
        }
    }
}
