using AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;
using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace AI_FraudeFinanciero_Core.Infrastructure.Repositories;

public class ModelosEntrenamientosRepository : IModeloEntrenamiento
{
    private readonly AppDBContext _dbContext;
    private readonly DbSet<ModeloEntrenamiento> _dbSetModeloEntrenamiento;
    private readonly IUnitOfWork _unitOfWork;

    public ModelosEntrenamientosRepository(AppDBContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _dbSetModeloEntrenamiento = _dbContext.Set<ModeloEntrenamiento>();
        _unitOfWork = unitOfWork;
    }

    public async Task<ModeloEntrenamiento> GetById(int modeloEntrenamientoId)
    {
        return await Task.FromResult(
           _dbSetModeloEntrenamiento
           .ToList()
           .FirstOrDefault(x => x.Modelo_Entrenamiento_Id == modeloEntrenamientoId)!
        );
    }

    public async Task<int> Add(ModeloEntrenamiento entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.Add(
                new ModeloEntrenamiento
                {
                    Modelo_Entrenamiento_Id = entity.Modelo_Entrenamiento_Id,
                    Nombre = entity.Nombre,
                    Creacion = entity.Creacion,
                    Modelo = entity.Modelo
                }
            );
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(1);
        }
        catch
        {
            return await Task.FromResult(0);
        }
    }

    public async Task<int> Delete(int modeloEntrenamientoId, CancellationToken cancellationToken)
    {
        try
        {
            var regCamara = new ModeloEntrenamiento
            {
                Modelo_Entrenamiento_Id = modeloEntrenamientoId
            };
            _dbSetModeloEntrenamiento.Remove(regCamara);
            var result = _unitOfWork.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(1);
        }
        catch
        {
            return await Task.FromResult(0);
        }
    }
}
