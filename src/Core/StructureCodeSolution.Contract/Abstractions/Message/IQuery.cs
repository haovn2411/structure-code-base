using MediatR;
using StructureCodeSolution.Contract.Abstractions.Shared;

namespace StructureCodeSolution.Contract.Abstractions.Message
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    { }
}
