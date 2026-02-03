using MediatR;
using StructureCodeSolution.Application.Abstractions.Shared;

namespace StructureCodeSolution.Application.Abstractions.Message
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    { }
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
    { }
}
