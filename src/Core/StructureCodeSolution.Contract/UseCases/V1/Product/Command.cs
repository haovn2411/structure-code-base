using StructureCodeSolution.Contract.Abstractions.Message;

namespace StructureCodeSolution.Contract.UseCases.V1.Product
{
    public static class Command
    {
        public record class CreateProduct(
            string Name,
            string Description,
            decimal Price) : ICommand;

    }
}
