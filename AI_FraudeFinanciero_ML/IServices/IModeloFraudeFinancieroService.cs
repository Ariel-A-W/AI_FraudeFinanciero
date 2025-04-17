using AI_FraudeFinanciero_ML.Models;

namespace AI_FraudeFinanciero_ML.IServices;

public interface IModeloFraudeFinancieroService
{
    MemoryStream Entrenamiento();
    TransaccionPrediction Predecir(TransaccionInput input);
}
