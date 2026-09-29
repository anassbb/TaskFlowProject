using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TaskFlow.Api.Common;

public static class ValidationExtensions
{
    /// <summary>
    /// Valide une requête avec son validator FluentValidation.
    /// Renvoie <c>null</c> si elle est valide, sinon une réponse 400 ValidationProblem listant les erreurs par champ.
    /// </summary>
    public static async Task<IResult?> ValidateRequestAsync<T>(this IValidator<T> validator, T request, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(request, ct);
        return result.IsValid ? null : TypedResults.ValidationProblem(result.ToDictionary());
    }
}
