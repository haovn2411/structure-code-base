namespace StructureCodeSolution.Domain.Abstractions.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
    }

    public abstract class NotFoundException : DomainException
    {
        protected NotFoundException(string message) : base(message) { }
    }

    public abstract class BadRequestException : DomainException
    {
        protected BadRequestException(string message) : base(message) { }
    }

    public abstract class ConflictException : DomainException
    {
        protected ConflictException(string message) : base(message) { }
    }
}
