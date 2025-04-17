using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AI_FraudeFinanciero_Core.Infrastructure.DBContexts;

public class AppDBContext : DbContext, IUnitOfWork
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        => TestConnection();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public Task<bool> SaveChangesAsync(bool acceptAllChangesOnSuccess, bool autoDetectChangesEnabled, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync(bool acceptAllChangesOnSuccess, bool autoDetectChangesEnabled, bool validateOnSaveEnabled, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    Task<bool> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<bool> IUnitOfWork.SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private void TestConnection()
    {
        try
        {
            if (Database.CanConnect())
            {
                Debug.WriteLine("Conexión a la base de datos exitosa.");
            }
            else
            {
                Debug.WriteLine("No se pudo establecer conexión con la base de datos.");
                Environment.Exit(1); // Cierra la aplicación si la conexión falla
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            Environment.Exit(1); // Cierra la aplicación en caso de error
        }
    }
}
