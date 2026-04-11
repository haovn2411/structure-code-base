using StructureCodeSolution.Application.Abstractions.Message;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts
{
    public static class Command
    {
        public record CreateCourseCommand(
            string Name,
            string? SummaryDescription,
            decimal Price,
            string Currency,
            string? ImageCode,
            int? CategoryId,
            int? LevelId) : ICommand;

        public record UpdateCourseCommand(
            Guid CourseId,
            string Name,
            string? SummaryDescription,
            decimal Price,
            string Currency,
            string? ImageCode) : ICommand;

        public record DeleteCourseCommand(Guid CourseId) : ICommand;

        public record AddVideoToCourseCommand(
            Guid CourseId,
            string Title,
            string? Description,
            TimeSpan Duration,
            int? Order) : ICommand;

        public record UpdateVideoCommand(
            Guid CourseId,
            Guid VideoId,
            string Title,
            string? Description,
            TimeSpan Duration) : ICommand;

        public record RemoveVideoCommand(
            Guid CourseId,
            Guid VideoId) : ICommand;

        public record ReorderVideoCommand(
            Guid CourseId,
            Guid VideoId,
            int NewOrder) : ICommand;

        public record PublishCourseCommand(Guid CourseId) : ICommand;

        public record RateCourseCommand(
            Guid CourseId,
            decimal Rating) : ICommand;

        public record EnrollStudentCommand(Guid CourseId) : ICommand;

        public record UpdateCourseCategoryCommand(
            Guid CourseId,
            int CategoryId) : ICommand;

        public record UpdateCourseLevelCommand(
            Guid CourseId,
            int LevelId) : ICommand;
    }
}