﻿using AI_FraudeFinanciero_API.Application;
using AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;
using AI_FraudeFinanciero_Core.Domain.Transacciones;
using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using AI_FraudeFinanciero_Core.Infrastructure.Repositories;
using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_ML.Services;

namespace AI_FraudeFinanciero_API.Infrastructure.DIP;

public static class DIPServices
{
    public static IServiceCollection AddDIPServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Añadir todas las dependencias aquí.

        // Establecer la unidad de trabajo.
        services.AddScoped<IUnitOfWork>(
            sp => (IUnitOfWork)sp.GetRequiredService<AppDBContext>()
        );

        services.AddScoped<ITransaccion, FraudeFinancieroRepository>();
        services.AddScoped<IModeloEntrenamiento, ModelosEntrenamientosRepository>();

        services.AddScoped<IModeloFFService, ModeloFFService>();
        services.AddScoped<IModeloFraudeFinancieroFastTreeRandomService, ModeloFraudeFinancieroFastTreeRandomService>();

        services.AddScoped<TransaccionUseCase>();

        return services;
    }
}
