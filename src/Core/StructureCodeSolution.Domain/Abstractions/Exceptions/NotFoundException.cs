using StructureCodeSolution.Domain.Abstractions.Exceptions;

namespace StructureCodeSolution.Domain.Exceptions.Commons
{
    public abstract class NotFoundException : DomainException
    {
        protected NotFoundException(string message)
            : base("Not Found", message)
        {
        }
    }
}