using MediatR;
using StructureCodeSolution.Application.Abstractions.Shared;

namespace StructureCodeSolution.Application.Abstractions.Message
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    { }
}
