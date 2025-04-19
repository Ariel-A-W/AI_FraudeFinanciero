using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_ML.Models;

namespace AI_FraudeFinanciero_API.Application;

public class TransaccionUseCase
{
    private readonly IModeloFFService _modeloFFService;

    public TransaccionUseCase(IModeloFFService modeloFFService)
    {
        _modeloFFService = modeloFFService;
    }

    public async Task<int> EntrenarTransacciones(CancellationToken cancellationToken)
    {
        return await _modeloFFService.Entrenamiento(cancellationToken);
    }


    public async Task<TransaccionResponseDTO> PredecirTransaccion(
        TransaccionRequestDTO transaccion,
        CancellationToken cancellationToken
    )
    { 
        var convertTrans = new TransaccionInput
        {
            Monto = transaccion.Monto,
            Frecuencia = transaccion.Frecuencia,
            TiempoTransaccion = transaccion.TiempoTransaccion,
            Canal = transaccion.Canal,
            Tipo = transaccion.Tipo,
            Origen = transaccion.Origen,
            Destino = transaccion.Destino
        };

        var resultado = await _modeloFFService.Predecir(convertTrans);

        return await Task.FromResult(
            new TransaccionResponseDTO
            { 
                Score = resultado.Score,
                Prediccion = resultado.PredictedLabel
            }
        );
    }
}
