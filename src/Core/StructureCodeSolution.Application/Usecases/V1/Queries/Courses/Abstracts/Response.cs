using StructureCodeSolution.Application.Abstractions.Shared;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts
{
    public static class Response
    {
        public record CourseResponse(
            Guid Id,
            string Name,
            string? SummaryDescription,
            decimal Price,
            string Currency,
            decimal StarRating,
            int NumberOfRatings,
            int NumberOfStudents,
            int NumberOfComments,
            int NumberOfLessons,
            int NumberOfHours,
            string? ImageCode,
            int? CategoryId,
            int? LevelId,
            List<VideoResponse> Videos,
            DateTimeOffset CreatedDate,
            DateTimeOffset? ModifiedDate);

        public record VideoResponse(
            Guid Id,
            string Title,
            string? Description,
            TimeSpan Duration,
            int Order,
            bool IsPublished,
            string? VideoUrl);

        public record CourseListResponse(
            List<CourseResponse> Courses,
            int PageIndex,
            int PageSize,
            int TotalCount,
            bool HasNextPage,
            bool HasPreviousPage);

        public record VideoListResponse(
            Guid CourseId,
            string CourseName,
            List<VideoResponse> Videos,
            int TotalVideos,
            int TotalHours);
    }
}
