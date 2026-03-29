using FluentValidation;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Validators
{
    public class AddVideoToCourseCommandValidator : AbstractValidator<Command.AddVideoToCourseCommand>
    {
        public AddVideoToCourseCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Course ID is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Video title is required")
                .MaximumLength(200).WithMessage("Video title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Duration)
                .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than zero");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order must be greater than or equal to 0")
                .When(x => x.Order.HasValue);
        }
    }
}
