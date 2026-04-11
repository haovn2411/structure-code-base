using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Exceptions;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses
{
    public class UpdateVideoCommandHandler : ICommandHandler<Command.UpdateVideoCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVideoCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Command.UpdateVideoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
                if (course is null)
                    return Result.Failure(new Error("Course.NotFound", $"Course with id '{request.CourseId}' was not found"));

                course.UpdateVideo(request.VideoId, request.Title, request.Description, request.Duration);
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
