using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Devices
{
    public class DeviceEvent : DomainEvent
    {
        public Guid DeviceId { get; private set; }
        public string Name { get; private set; }

        public string Description { get; private set; }
        public DeviceEvent(Guid deviceId, string name, string description)
        {
            DeviceId = deviceId;
            Name = name;
            Description = description;
        }
    }
}
