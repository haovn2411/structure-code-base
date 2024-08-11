using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Devices;

namespace StructureCodeSolution.Persistence.Repositories
{
    public class DeviceRepository : RepositoryBase<Device, Guid>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }
    }
}
