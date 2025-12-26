using StructureCodeSolution.Contract.Abstractions.Message;

namespace StructureCodeSolution.Contract.UseCases.V1.Device
{
    public static class Command
    {
        public record class CreateDeviceCommand(
            string Name,
            string Description) : ICommand;
    }
}
