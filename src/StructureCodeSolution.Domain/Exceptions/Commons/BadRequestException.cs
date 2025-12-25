namespace StructureCodeSolution.Domain.Exceptions.Commons
{
    public abstract class BadRequestException : DomainException
    {
        protected BadRequestException(string message)
            : base("Bad Request", message)
        {
        }
    }

}
