using FluentValidation;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Validators
{
    public class UpdateCourseCommandValidator : AbstractValidator<Command.UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Course ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(200).WithMessage("Course name must not exceed 200 characters");

            RuleFor(x => x.SummaryDescription)
                .MaximumLength(1000).WithMessage("Summary description must not exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.SummaryDescription));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required")
                .Length(3).WithMessage("Currency must be 3 characters (e.g., USD, EUR)");

            RuleFor(x => x.ImageCode)
                .MaximumLength(500).WithMessage("Image code must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.ImageCode));
        }
    }
}
