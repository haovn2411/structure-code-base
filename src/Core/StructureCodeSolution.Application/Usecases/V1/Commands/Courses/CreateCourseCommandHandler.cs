using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Courses;
using StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses
{
    public class CreateCourseCommandHandler : ICommandHandler<Command.CreateCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Command.CreateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if course name already exists
                var nameExists = await _courseRepository.ExistsByNameAsync(request.Name, cancellationToken);
                if (nameExists)
                {
                    return Result.Failure<Guid>(new Error(
                        "Course.NameAlreadyExists",
                        $"Course with name '{request.Name}' already exists"));
                }

                // Create Money value object
                var price = Money.Create(request.Price, request.Currency);

                // Create course
                var course = Course.Create(
                    request.Name,
                    request.SummaryDescription,
                    price,
                    request.ImageCode,
                    request.CategoryId,
                    request.LevelId);

                _courseRepository.Add(course);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(course.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(new Error(ex.GetType().Name, ex.Message));
            }
        }
    }
}