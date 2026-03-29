using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    internal class VideoAddedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; private set; }
        public Guid VideoId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int Order { get; private set; }

        public VideoAddedDomainEvent(Guid courseId, Guid videoId, string title, string? description, TimeSpan duration, int order)
        {
            CourseId = courseId;
            VideoId = videoId;
            Title = title;
            Description = description;
            Duration = duration;
            Order = order;
        }
    }
}