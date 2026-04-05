namespace StructureCodeSolution.Domain.Abstractions.Exceptions
{
    public abstract class ConflictException : DomainException
    {
        protected ConflictException(string message)
            : base("Conflict", message)
        {
        }
    }
}