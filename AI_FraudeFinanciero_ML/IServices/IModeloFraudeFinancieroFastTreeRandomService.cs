using AI_FraudeFinanciero_ML.Models;

namespace AI_FraudeFinanciero_ML.IServices;

public interface IModeloFraudeFinancieroFastTreeRandomService
{
    Task<MemoryStream> Entrenamiento();
    TransaccionPrediction Predecir(TransaccionInput input);
}
