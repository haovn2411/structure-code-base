using StructureCodeSolution.Application.Abstractions.Message;

namespace StructureCodeSolution.Application.Usecases.V1.Products.Commands.Commons
{
    public static class Command
    {
        public record class CreateProductCommand(
            string Name,
            decimal Price,
            string Description) : ICommand;

        public record class UpdateProductCommand(
            Guid Id,
            string Name,
            decimal Price,
            string Description) : ICommand;

        public record class DeleteProductCommand(Guid Id) : ICommand;
    }
}
