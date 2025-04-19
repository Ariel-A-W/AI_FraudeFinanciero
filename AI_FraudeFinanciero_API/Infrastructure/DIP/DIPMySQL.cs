using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace AI_FraudeFinanciero_API.Infrastructure.DIP;

public static class DIPMySQL
{
    //public static IServiceCollection AddApplication(
    //    this IServiceCollection services
    //)
    //{
    //    return services;
    //}

    public static IServiceCollection AddMySQLConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Sección para configura MySQL Server.
        var connectionString = configuration.GetConnectionString("FraudeFinanciero")
             ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppDBContext>(
            options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(
                        new Version(8, 0, 21)
                    )
                )
        );

        return services;
    }
}
