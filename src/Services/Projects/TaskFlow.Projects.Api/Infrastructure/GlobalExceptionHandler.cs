using Microsoft.AspNetCore.Diagnostics;

namespace TaskFlow.Projects.Api.Infrastructure;

/// <summary>
/// Filet de sécurité pour les exceptions non prévues : log complet côté serveur,
/// réponse 500 générique côté client (aucun détail technique exposé).
/// </summary>
internal sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
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
