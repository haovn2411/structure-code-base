using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using StructureCodeSolution.Domain.Aggregates.Devices;

namespace StructureCodeSolution.Domain.Abstractions.Repositories
{
    public interface IDeviceRepository : IRepositoryBase<Device, Guid>
    {
    }
}
