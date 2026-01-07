using MediatR;
using StructureCodeSolution.Contract.Abstractions.Shared;

namespace StructureCodeSolution.Contract.Abstractions.Message
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    { }
}
