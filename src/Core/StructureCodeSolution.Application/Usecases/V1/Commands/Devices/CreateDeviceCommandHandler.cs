using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Devices.Abstracts;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Devices;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Devices
{
    public class CreateDeviceCommandHandler : ICommandHandler<Command.CreateDeviceCommand>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDeviceCommandHandler(IUnitOfWork unitOfWork, IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(Command.CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = Device.Create(request.Name, request.Description);
            _deviceRepository.Add(device);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
