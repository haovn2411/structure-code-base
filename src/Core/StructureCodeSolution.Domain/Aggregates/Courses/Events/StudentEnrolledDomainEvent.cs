using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class StudentEnrolledDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public int TotalStudents { get; }

        public StudentEnrolledDomainEvent(Guid courseId, int totalStudents)
        {
            CourseId = courseId;
            TotalStudents = totalStudents;
        }
    }
}
