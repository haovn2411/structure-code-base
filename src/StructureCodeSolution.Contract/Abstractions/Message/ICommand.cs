using MediatR;
using StructureCodeSolution.Contract.Abstractions.Shared;

namespace StructureCodeSolution.Contract.Abstractions.Message
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
