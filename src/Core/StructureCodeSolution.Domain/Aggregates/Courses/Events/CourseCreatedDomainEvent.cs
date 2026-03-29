using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.Events
{
    internal class CourseCreatedDomainEvent : DomainEvent
    {
        public Guid CourseId { get; private set; }
        public string Name { get; private set; }
        public string? SummaryDescription { get; private set; }
        public decimal Price { get; private set; }
        public string? ImageCode { get; private set; }

        public CourseCreatedDomainEvent(Guid courseId, string name, string? summaryDescription,
            decimal price, string? imageCode)
        {
            CourseId = courseId;
            Name = name;
            SummaryDescription = summaryDescription;
            Price = price;
            ImageCode = imageCode;
        }
    }
}