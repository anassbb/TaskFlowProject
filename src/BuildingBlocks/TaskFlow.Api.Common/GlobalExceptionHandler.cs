using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TaskFlow.Api.Common;

/// <summary>
/// Filet de sécurité pour les exceptions non prévues : log complet côté serveur,
/// réponse 500 générique côté client (aucun détail technique exposé).
/// </summary>
public sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Requête mal formée (JSON invalide, corps illisible...) : c'est une erreur du CLIENT -> 400, pas 500.
        if (exception is BadHttpRequestException badRequest)
        {
            httpContext.Response.StatusCode = badRequest.StatusCode;
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = { Title = "Requête invalide.", Status = badRequest.StatusCode },
            });
        }

        LogUnhandledException(logger, httpContext.Request.Path, exception);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Title = "Une erreur inattendue est survenue.",
                Status = StatusCodes.Status500InternalServerError,
            },
        });
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Exception non gérée sur {Path}")]
    private static partial void LogUnhandledException(ILogger logger, PathString path, Exception exception);
}
