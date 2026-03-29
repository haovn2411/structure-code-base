using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class VideoRemovedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public Guid VideoId { get; }

        public VideoRemovedDomainEvent(Guid courseId, Guid videoId)
        {
            CourseId = courseId;
            VideoId = videoId;
        }
    }
}
