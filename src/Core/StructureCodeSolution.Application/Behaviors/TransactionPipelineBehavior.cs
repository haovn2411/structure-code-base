using MediatR;
using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Application.Behaviors
{
    public sealed class TransactionPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionPipelineBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!IsCommand())
            {
                // In case TRequest is QueryRequest just ignore
                return await next(cancellationToken);
            }
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next(cancellationToken); // Execute the Handler
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return response;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private bool IsCommand()
            => typeof(TRequest).Name.EndsWith("Command");
    }
}
