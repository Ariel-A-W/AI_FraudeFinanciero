namespace AI_FraudeFinanciero_API.Application.Abstractions;

public class ValidationErrorDetails
{
    public string Title { get; set; } = "Errores de validación";
    public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
    public IEnumerable<string> Errors { get; set; }

    public ValidationErrorDetails() { }

    public ValidationErrorDetails(IEnumerable<string> errors)
    {
        Errors = errors;
    }
}