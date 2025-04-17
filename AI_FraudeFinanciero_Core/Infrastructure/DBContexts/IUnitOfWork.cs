namespace AI_FraudeFinanciero_Core.Infrastructure.DBContexts;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(bool acceptAllChangesOnSuccess, bool autoDetectChangesEnabled, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(bool acceptAllChangesOnSuccess, bool autoDetectChangesEnabled, bool validateOnSaveEnabled, CancellationToken cancellationToken = default);
}
