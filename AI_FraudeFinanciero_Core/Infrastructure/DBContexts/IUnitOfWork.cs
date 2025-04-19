namespace AI_FraudeFinanciero_Core.Infrastructure.DBContexts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
