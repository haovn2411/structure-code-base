namespace StructureCodeSolution.Application.Abstractions.Identity
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
    }
}
