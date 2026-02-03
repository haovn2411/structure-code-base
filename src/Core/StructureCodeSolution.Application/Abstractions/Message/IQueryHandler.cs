using MediatR;
using StructureCodeSolution.Application.Abstractions.Shared;

namespace StructureCodeSolution.Application.Abstractions.Message
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    { }
}
