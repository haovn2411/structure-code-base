using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Devices
{
    public class Device : AggregateRoot<Guid>
    {
        public string Name { get; private set; }

        public string Description { get; private set; }
        private Device(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }
        public static Device Create(string name, string description)
        {
            var device = new Device(name, description);
            device.Raise(new DeviceEvent(device.Id, name, description));
            return device;
        }
    }
}
