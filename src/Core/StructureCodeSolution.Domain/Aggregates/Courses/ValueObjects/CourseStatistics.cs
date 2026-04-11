using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects
{
    public sealed class CourseStatistics : ValueObject
    {
        public int NumberOfStudents { get; private set; }
        public int NumberOfComments { get; private set; }
        public int NumberOfLessons { get; private set; }
        public decimal TotalHours { get; private set; }

        private CourseStatistics() { }

        private CourseStatistics(int numberOfStudents, int numberOfComments, int numberOfLessons, decimal totalHours)
        {
            NumberOfStudents = numberOfStudents;
            NumberOfComments = numberOfComments;
            NumberOfLessons = numberOfLessons;
            TotalHours = totalHours;
        }

        public static CourseStatistics Create() => new CourseStatistics(0, 0, 0, 0);

        public void IncrementStudents()
            => NumberOfStudents++;

        public void IncrementComments()
            => NumberOfComments++;

        public void UpdateLessonsAndHours(int lessons, decimal hours)
        {
            if (lessons < 0 || hours < 0)
                throw new ArgumentException("Lessons and hours cannot be negative");

            NumberOfLessons = lessons;
            TotalHours = Math.Round(hours, 1);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NumberOfStudents;
            yield return NumberOfComments;
            yield return NumberOfLessons;
            yield return TotalHours;
        }
    }
}
