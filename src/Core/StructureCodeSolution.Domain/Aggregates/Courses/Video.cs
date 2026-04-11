using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Aggregates.Courses.Exceptions;

namespace StructureCodeSolution.Domain.Aggregates.Courses
{
    public class Video : EntityAuditBase<Guid>
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int Order { get; private set; }
        public bool IsPublished { get; private set; }
        public string? VideoUrl { get; private set; }

        private Video()
        { }

        private Video(string title, string? description, TimeSpan duration, int order)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new VideoException.InvalidVideoTitleException();

            if (duration <= TimeSpan.Zero)
                throw new VideoException.InvalidVideoDurationException();

            if (order < 0)
                throw new VideoException.InvalidVideoOrderException(order);

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Duration = duration;
            Order = order;
            IsPublished = false;
        }

        internal static Video Create(string title, string? description, TimeSpan duration, int order)
        {
            return new Video(title, description, duration, order);
        }

        public void UpdateDetails(string title, string? description, TimeSpan duration)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new VideoException.InvalidVideoTitleException();

            if (duration <= TimeSpan.Zero)
                throw new VideoException.InvalidVideoDurationException();

            Title = title;
            Description = description;
            Duration = duration;
        }

        public void UpdateOrder(int newOrder)
        {
            if (newOrder < 0)
                throw new VideoException.InvalidVideoOrderException(newOrder);

            Order = newOrder;
        }

        public void Publish(string videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                throw new ArgumentException("Video URL cannot be empty", nameof(videoUrl));

            IsPublished = true;
            VideoUrl = videoUrl;
        }

        public void Unpublish()
        {
            IsPublished = false;
        }
    }
}