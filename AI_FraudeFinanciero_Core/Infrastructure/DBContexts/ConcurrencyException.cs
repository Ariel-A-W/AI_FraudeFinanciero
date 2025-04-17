namespace AI_FraudeFinanciero_Core.Infrastructure.DBContexts;

public class ConcurrencyException : Exception
{
    public ConcurrencyException(
        string message, Exception innerException
    ) : base(message, innerException)
    { }
}
