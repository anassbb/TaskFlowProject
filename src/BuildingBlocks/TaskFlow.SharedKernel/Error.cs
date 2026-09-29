namespace TaskFlow.SharedKernel;

/// <summary>
/// Erreur métier attendue (règle violée, ressource introuvable...).
/// Elle est renvoyée dans un <see cref="Result"/> au lieu d'être levée comme exception.
/// </summary>
public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
    public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
    public static Error Forbidden(string code, string description) => new(code, description, ErrorType.Forbidden);
}

/// <summary>Catégorie d'erreur, traduite en code HTTP par l'API (400, 404, 409, 403...).</summary>
public enum ErrorType
{
    Failure,
    Validation,
    NotFound,
    Conflict,
    Forbidden,
}
