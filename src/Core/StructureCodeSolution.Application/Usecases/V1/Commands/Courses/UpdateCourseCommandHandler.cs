using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Exceptions;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses
{
    public class UpdateCourseCommandHandler : ICommandHandler<Command.UpdateCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Command.UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
                if (course is null)
                {
                    return Result.Failure(new Error(
                        "Course.NotFound",
                        $"Course with id '{request.CourseId}' was not found"));
                }

                course.UpdateCourse(
                    request.Name,
                    request.SummaryDescription,
                    request.Price,
                    request.Currency,
                    request.ImageCode);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(new Error(ex.GetType().Name, ex.Message));
            }
        }
    }
}
