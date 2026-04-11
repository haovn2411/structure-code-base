using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Exceptions;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Courses
{
    public class RemoveVideoCommandHandler : ICommandHandler<Command.RemoveVideoCommand>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveVideoCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Command.RemoveVideoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
                if (course is null)
                    return Result.Failure(new Error("Course.NotFound", $"Course with id '{request.CourseId}' was not found"));

                course.RemoveVideo(request.VideoId);
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
