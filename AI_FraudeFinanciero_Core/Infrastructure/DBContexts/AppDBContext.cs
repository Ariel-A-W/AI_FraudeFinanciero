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

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(
                "Infrastructure: Fallo en la concurrencia de los datos. Detalle:", ex
            );
        }
        catch (Exception ex)
        {
            throw new ConcurrencyException(
                "Infrastructure: Fallo genérico. Detalle:", ex
            );
        }
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
                //Environment.Exit(1); // Cierra la aplicación si la conexión falla
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            //Environment.Exit(1); // Cierra la aplicación en caso de error
        }
    }
}
