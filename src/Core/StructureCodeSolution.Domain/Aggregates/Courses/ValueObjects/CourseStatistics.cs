using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects
{
    public sealed class CourseStatistics : ValueObject
    {
        public int NumberOfStudents { get; private set; }
        public int NumberOfComments { get; private set; }
        public int NumberOfLessons { get; private set; }
        public int NumberOfHours { get; private set; }

        private CourseStatistics() { }

        private CourseStatistics(int numberOfStudents, int numberOfComments, int numberOfLessons, int numberOfHours)
        {
            NumberOfStudents = numberOfStudents;
            NumberOfComments = numberOfComments;
            NumberOfLessons = numberOfLessons;
            NumberOfHours = numberOfHours;
        }

        public static CourseStatistics Create() => new CourseStatistics(0, 0, 0, 0);

        public void IncrementStudents()
            => NumberOfStudents++;

        public void IncrementComments()
            => NumberOfComments++;

        public void UpdateLessonsAndHours(int lessons, int hours)
        {
            if (lessons < 0 || hours < 0)
                throw new ArgumentException("Lessons and hours cannot be negative");

            NumberOfLessons = lessons;
            NumberOfHours = hours;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NumberOfStudents;
            yield return NumberOfComments;
            yield return NumberOfLessons;
            yield return NumberOfHours;
        }
    }
}
