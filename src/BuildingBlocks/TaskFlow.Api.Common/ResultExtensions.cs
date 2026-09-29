using Microsoft.AspNetCore.Http;
using TaskFlow.SharedKernel;

namespace TaskFlow.Api.Common;

/// <summary>
/// Traduit un <see cref="Result"/> en échec vers une réponse HTTP ProblemDetails.
/// Usage dans un endpoint : <c>return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblem();</c>
/// </summary>
public static class ResultExtensions
{
    public static IResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Un résultat en succès ne peut pas être converti en erreur.");
        }

        var error = result.Error;
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status422UnprocessableEntity,
        };

        return TypedResults.Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: status,
            extensions: new Dictionary<string, object?> { ["errorCode"] = error.Code });
    }
}
