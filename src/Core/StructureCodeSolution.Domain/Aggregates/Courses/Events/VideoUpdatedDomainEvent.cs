using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    public class VideoUpdatedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; }
        public Guid VideoId { get; }
        public string Title { get; }
        public string? Description { get; }
        public TimeSpan Duration { get; }

        public VideoUpdatedDomainEvent(Guid courseId, Guid videoId, string title, string? description, TimeSpan duration)
        {
            CourseId = courseId;
            VideoId = videoId;
            Title = title;
            Description = description;
            Duration = duration;
        }
    }
}
