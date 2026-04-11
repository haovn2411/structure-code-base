using MediatR;
using Microsoft.AspNetCore.Mvc;
using StructureCodeSolution.Application.Abstractions.Shared;

namespace StructureCodeSolution.API.Abstractions
{
    public class ApiController : ControllerBase
    {
        protected readonly ISender Sender;

        protected ApiController(ISender sender) => Sender = sender;

        protected IActionResult HandleSuccess(Result result, int statusCode = StatusCodes.Status200OK)
            => StatusCode(statusCode, result);

        protected IActionResult HandlerFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request", StatusCodes.Status400BadRequest,
                        result.Error))
        };
        private static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Message,
                Status = status,
                Extensions = { { nameof(errors), errors } }
            };
    }
}
