using StructureCodeSolution.Domain.Abstractions.Exceptions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Exceptions
{
    public static class CourseException
    {
        public class CourseNotFoundException : NotFoundException
        {
            public CourseNotFoundException(Guid courseId)
                : base($"The course with id '{courseId}' was not found") { }
        }

        public class CourseNameAlreadyExistsException : ConflictException
        {
            public CourseNameAlreadyExistsException(string name)
                : base($"Course with name '{name}' already exists") { }
        }

        public class InvalidCourseNameException : BadRequestException
        {
            public InvalidCourseNameException()
                : base("Course name cannot be empty") { }
        }

        public class InvalidPriceException : BadRequestException
        {
            public InvalidPriceException()
                : base("Price cannot be negative") { }
        }

        public class InvalidCategoryException : BadRequestException
        {
            public InvalidCategoryException(int categoryId)
                : base($"Invalid category id: {categoryId}") { }
        }

        public class InvalidLevelException : BadRequestException
        {
            public InvalidLevelException(int levelId)
                : base($"Invalid level id: {levelId}") { }
        }

        public class CannotPublishWithoutVideosException : BadRequestException
        {
            public CannotPublishWithoutVideosException()
                : base("Cannot publish course without any videos") { }
        }
    }

    public static class VideoException
    {
        public class VideoNotFoundException : NotFoundException
        {
            public VideoNotFoundException(Guid videoId)
                : base($"The video with id '{videoId}' was not found") { }
        }

        public class DuplicateVideoOrderException : ConflictException
        {
            public DuplicateVideoOrderException(int order)
                : base($"A video with order {order} already exists") { }
        }

        public class InvalidVideoOrderException : BadRequestException
        {
            public InvalidVideoOrderException(int order)
                : base($"Invalid video order: {order}") { }
        }

        public class InvalidVideoTitleException : BadRequestException
        {
            public InvalidVideoTitleException()
                : base("Video title cannot be empty") { }
        }

        public class InvalidVideoDurationException : BadRequestException
        {
            public InvalidVideoDurationException()
                : base("Video duration must be greater than zero") { }
        }
    }

    public static class RatingException
    {
        public class InvalidRatingException : BadRequestException
        {
            public InvalidRatingException()
                : base("Rating must be between 0 and 5") { }
        }
    }
}
