using FluentValidation;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Validators
{
    public class RateCourseCommandValidator : AbstractValidator<Command.RateCourseCommand>
    {
        public RateCourseCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Course ID is required");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5");
        }
    }
}
