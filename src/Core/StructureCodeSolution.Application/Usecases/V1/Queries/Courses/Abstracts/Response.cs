namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts
{
    public static class Response
    {
        public record CourseResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? SummaryDescription { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }
            public decimal StarRating { get; set; }
            public int NumberOfRatings { get; set; }
            public int NumberOfStudents { get; set; }
            public int NumberOfComments { get; set; }
            public int NumberOfLessons { get; set; }
            public decimal TotalHours { get; set; }
            public string? ImageCode { get; set; }
            public int? CategoryId { get; set; }
            public int? LevelId { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
            public DateTimeOffset? ModifiedDate { get; set; }
        }

        public record VideoResponse
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string? Description { get; set; }
            public TimeSpan Duration { get; set; }
            public int Order { get; set; }
            public bool IsPublished { get; set; }
            public string? VideoUrl { get; set; }
        }
    }
}