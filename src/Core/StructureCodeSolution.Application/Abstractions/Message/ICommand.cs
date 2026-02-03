using MediatR;
using StructureCodeSolution.Application.Abstractions.Shared;

namespace StructureCodeSolution.Application.Abstractions.Message
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
