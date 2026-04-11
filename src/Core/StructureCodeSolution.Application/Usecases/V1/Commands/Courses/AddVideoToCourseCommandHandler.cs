using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Exceptions;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses
{
    public class AddVideoToCourseCommandHandler : ICommandHandler<Command.AddVideoToCourseCommand, Guid>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddVideoToCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(Command.AddVideoToCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(
                    request.CourseId);
                if (course is null)
                {
                    return Result.Failure<Guid>(new Error(
                        "Course.NotFound",
                        $"Course with id '{request.CourseId}' was not found"));
                }

                course.AddVideo(
                    request.Title,
                    request.Description,
                    request.Duration,
                    request.Order);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var addedVideo = course.Videos.OrderByDescending(v => v.CreatedDate).First();
                return Result.Success(addedVideo.Id);
            }
            catch (DomainException ex)
            {
                return Result.Failure<Guid>(new Error(ex.GetType().Name, ex.Message));
            }
        }
    }
}