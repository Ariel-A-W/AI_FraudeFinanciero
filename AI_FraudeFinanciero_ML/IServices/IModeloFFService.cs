using AI_FraudeFinanciero_ML.Models;

namespace AI_FraudeFinanciero_ML.IServices;

public interface IModeloFFService
{
    Task Liberar(CancellationToken cancellationToken);
    Task<int> Entrenamiento(CancellationToken cancellationToken);
    Task<TransaccionPrediction> Predecir(TransaccionInput input);
}
