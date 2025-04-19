namespace AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;

public interface IModeloEntrenamiento
{
    Task<ModeloEntrenamiento> GetById(int modeloEntrenamientoId);
    Task<int> Add(ModeloEntrenamiento entity, CancellationToken cancellationToken);
    Task<int> Delete(int modeloEntrenamientoId, CancellationToken cancellationToken); 
}
