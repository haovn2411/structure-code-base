using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class CoursePublishedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public string CourseName { get; }

        public CoursePublishedDomainEvent(Guid courseId, string courseName)
        {
            CourseId = courseId;
            CourseName = courseName;
        }
    }
}
