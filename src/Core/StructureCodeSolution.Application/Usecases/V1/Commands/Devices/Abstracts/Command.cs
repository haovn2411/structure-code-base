using StructureCodeSolution.Application.Abstractions.Message;

namespace StructureCodeSolution.Application.Usecases.V1.Commands.Devices.Abstracts
{
    public static class Command
    {
        public record class CreateDeviceCommand(
            string Name,
            string Description) : ICommand;
    }
}
