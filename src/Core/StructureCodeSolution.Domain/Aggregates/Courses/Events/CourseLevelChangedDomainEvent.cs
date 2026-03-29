using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class CourseLevelChangedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public int LevelId { get; }

        public CourseLevelChangedDomainEvent(Guid courseId, int levelId)
        {
            CourseId = courseId;
            LevelId = levelId;
        }
    }
}
