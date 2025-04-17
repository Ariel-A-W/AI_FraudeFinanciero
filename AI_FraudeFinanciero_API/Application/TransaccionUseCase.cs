using AI_FraudeFinanciero_Core.Domain;
using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_ML.Models;
using AI_FraudeFinanciero_ML.Services;

namespace AI_FraudeFinanciero_API.Application;

public class TransaccionUseCase
{
    private readonly IModeloFraudeFinancieroService _fraudeFinanciero;

    public TransaccionUseCase(IModeloFraudeFinancieroService fraudeFinanciero)
    {
        _fraudeFinanciero = fraudeFinanciero;
    }

    public TransaccionPrediction PredecirTransaccion(TransaccionInput input)
    {
        var resultado = _fraudeFinanciero.Predecir(input);
        return resultado;
    }
}
