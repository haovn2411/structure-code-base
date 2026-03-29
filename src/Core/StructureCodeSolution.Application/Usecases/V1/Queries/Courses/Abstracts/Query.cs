using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts
{
    public static class Query
    {
        public record GetCourseByIdQuery(Guid CourseId) : IQuery<Response.CourseResponse>;

        public record GetAllCoursesQuery(
            int PageIndex = 1,
            int PageSize = 10,
            string? SearchTerm = null,
            int? CategoryId = null,
            int? LevelId = null) : IQuery<Response.CourseListResponse>;

        public record GetCourseVideosQuery(Guid CourseId) : IQuery<Response.VideoListResponse>;
    }
}
