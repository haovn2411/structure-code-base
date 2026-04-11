using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Aggregates.Courses.Events;
using StructureCodeSolution.Domain.Aggregates.Courses.Exceptions;
using StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects;

namespace StructureCodeSolution.Domain.Aggregates.Courses
{
    public class Course : AggregateAuditedRoot<Guid>
    {
        private readonly List<Video> _videos = new();

        public string Name { get; private set; }
        public string? SummaryDescription { get; private set; }
        public Money Price { get; private set; }
        public Rating Rating { get; private set; }
        public CourseStatistics Statistics { get; private set; }
        public string? ImageCode { get; private set; }

        // Foreign Keys to other aggregates
        public int? CategoryId { get; private set; }

        public int? LevelId { get; private set; }

        // Collection - chỉ expose read-only
        public IReadOnlyCollection<Video> Videos => _videos.AsReadOnly();

        private Course()
        { }

        private Course(string name, string? summaryDescription, Money price, string? imageCode, int? categoryId, int? levelId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CourseException.InvalidCourseNameException();

            Id = Guid.NewGuid();
            Name = name;
            SummaryDescription = summaryDescription;
            Price = price ?? Money.Free();
            Rating = Rating.Create();
            Statistics = CourseStatistics.Create();
            ImageCode = imageCode;
            CategoryId = categoryId;
            LevelId = levelId;
        }

        public static Course Create(string name, string? summaryDescription, Money price, string? imageCode, int? categoryId = null, int? levelId = null)
        {
            var course = new Course(name, summaryDescription, price, imageCode, categoryId, levelId);
            course.Raise(new CourseCreatedDomainEvent(
                course.Id,
                name,
                summaryDescription,
                price.Amount,
                imageCode));
            return course;
        }

        // ===== VIDEO MANAGEMENT =====
        public void AddVideo(string title, string? description, TimeSpan duration, int? order = null)
        {
            //// Nếu không truyền order, tự động tính order tiếp theo
            //var videoOrder = order ?? _videos.Count;

            //// Kiểm tra trùng order
            //if (_videos.Any(v => v.Order == videoOrder))
            //    throw new VideoException.DuplicateVideoOrderException(videoOrder);

            var video = Video.Create(title, description, duration, order ?? 0);
            _videos.Add(video);

            // Update statistics
            RecalculateStatistics();

            Raise(new VideoAddedDomainEvent(this.Id, video.Id, title, description, duration, order ?? 0));
        }

        public void RemoveVideo(Guid videoId)
        {
            var video = _videos.FirstOrDefault(v => v.Id == videoId);
            if (video == null)
                throw new VideoException.VideoNotFoundException(videoId);

            _videos.Remove(video);

            // Reorder remaining videos
            ReorderVideos();
            RecalculateStatistics();

            Raise(new VideoRemovedDomainEvent(this.Id, videoId));
        }

        public void UpdateVideo(Guid videoId, string title, string? description, TimeSpan duration)
        {
            var video = _videos.FirstOrDefault(v => v.Id == videoId);
            if (video == null)
                throw new VideoException.VideoNotFoundException(videoId);

            video.UpdateDetails(title, description, duration);
            RecalculateStatistics();

            Raise(new VideoUpdatedDomainEvent(this.Id, videoId, title, description, duration));
        }

        public void ReorderVideo(Guid videoId, int newOrder)
        {
            var video = _videos.FirstOrDefault(v => v.Id == videoId);
            if (video == null)
                throw new VideoException.VideoNotFoundException(videoId);

            if (newOrder < 0 || newOrder >= _videos.Count)
                throw new VideoException.InvalidVideoOrderException(newOrder);

            var currentOrder = video.Order;
            if (currentOrder == newOrder)
                return;

            // Cập nhật order của các video khác
            foreach (var v in _videos.Where(v => v.Id != videoId))
            {
                if (currentOrder < newOrder && v.Order > currentOrder && v.Order <= newOrder)
                    v.UpdateOrder(v.Order - 1);
                else if (currentOrder > newOrder && v.Order >= newOrder && v.Order < currentOrder)
                    v.UpdateOrder(v.Order + 1);
            }

            video.UpdateOrder(newOrder);
        }

        private void ReorderVideos()
        {
            var orderedVideos = _videos.OrderBy(v => v.Order).ToList();
            for (int i = 0; i < orderedVideos.Count; i++)
            {
                orderedVideos[i].UpdateOrder(i);
            }
        }

        private void RecalculateStatistics()
        {
            var totalLessons = _videos.Count;
            var totalHours = (int)Math.Ceiling(_videos.Sum(v => v.Duration.TotalHours));

            Statistics.UpdateLessonsAndHours(totalLessons, totalHours);
        }

        // ===== COURSE MANAGEMENT =====
        public void UpdateCourse(string name, string? summaryDescription, decimal price, string currency, string? imageCode)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CourseException.InvalidCourseNameException();

            Name = name;
            SummaryDescription = summaryDescription;
            Price.UpdateAmount(price);
            Price.UpdateCurrency(currency);
            ImageCode = imageCode;

            Raise(new CourseUpdatedDomainEvent(this.Id, name, summaryDescription, price, imageCode));
        }

        public void UpdateCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new CourseException.InvalidCategoryException(categoryId);

            CategoryId = categoryId;
            Raise(new CourseCategoryChangedDomainEvent(this.Id, categoryId));
        }

        public void UpdateLevel(int levelId)
        {
            if (levelId <= 0)
                throw new CourseException.InvalidLevelException(levelId);

            LevelId = levelId;
            Raise(new CourseLevelChangedDomainEvent(this.Id, levelId));
        }

        // ===== RATING & ENGAGEMENT =====
        public void AddRating(decimal starRating)
        {
            Rating.AddRating(starRating);
            Raise(new CourseRatedDomainEvent(this.Id, starRating, Rating.StarRating, Rating.NumberOfRatings));
        }

        public void EnrollStudent()
        {
            Statistics.IncrementStudents();
            Raise(new StudentEnrolledDomainEvent(this.Id, Statistics.NumberOfStudents));
        }

        public void AddComment()
        {
            Statistics.IncrementComments();
        }

        // ===== PUBLISHING =====
        public void Publish()
        {
            if (_videos.Count == 0)
                throw new CourseException.CannotPublishWithoutVideosException();

            Raise(new CoursePublishedDomainEvent(this.Id, Name));
        }
    }
}