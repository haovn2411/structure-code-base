using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class CourseCategoryChangedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public int CategoryId { get; }

        public CourseCategoryChangedDomainEvent(Guid courseId, int categoryId)
        {
            CourseId = courseId;
            CategoryId = categoryId;
        }
    }
}
