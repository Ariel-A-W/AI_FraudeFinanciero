using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_ML.Models;
using System.Threading;

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
        
        await _modeloFFService.Entrenamiento(cancellationToken);

        return await Task.FromResult(new TransaccionResponseDTO
        {
            Score = 8.34f,
            Prediccion = false
        });
    }


    //private readonly IModeloFraudeFinancieroService _fraudeFinanciero;
    //private readonly IModeloFraudeFinancieroFastTreeRandomService _fraudeFinancieroFastTreeRandom;

    //public TransaccionUseCase(
    //    IModeloFraudeFinancieroService fraudeFinanciero, 
    //    IModeloFraudeFinancieroFastTreeRandomService fraudeFinancieroFastTreeRandom
    //)
    //{
    //    _fraudeFinanciero = fraudeFinanciero;
    //    _fraudeFinancieroFastTreeRandom = fraudeFinancieroFastTreeRandom;
    //}

    //public async Task<TransaccionResponseDTO> PredecirTransaccion(
    //    TransaccionRequestDTO transaccion
    //) 
    //{
    //    /**
    //     * Selección del Algorítmo: 
    //     * 
    //     * 1 - Utiliza el algoritmo de ML.NET tipo "BinaryClassification" medioante 
    //     * el algortimo "SdcaLogisticRegression". 
    //     * 
    //     * 2 - Uilita el algoritmo de ML.NET tipo "FastTree" medioante el algortimo 
    //     * FastFree y RandomForest. Esta opción permite el uso de una combinación 
    //     * de ambos algoritmos más el de SdacLogisticRegression por defecto.
    //     */

    //    int selectAlgoth = 1; 
    //    //int selectAlgoth = transaccion.Algoritmo;

    //    var convertTrans = new TransaccionInput
    //    {
    //        Monto = transaccion.Monto,
    //        Frecuencia = transaccion.Frecuencia,
    //        TiempoTransaccion = transaccion.TiempoTransaccion,
    //        Canal = transaccion.Canal,
    //        Tipo = transaccion.Tipo,
    //        Origen = transaccion.Origen,
    //        Destino = transaccion.Destino
    //    };

    //    switch (selectAlgoth)
    //    {
    //        case 1:
    //            var resultado1 = _fraudeFinanciero.Predecir(convertTrans);

    //            return await Task.FromResult(new TransaccionResponseDTO
    //            {
    //                Score = resultado1.Score,
    //                Prediccion = resultado1.PredictedLabel
    //            });
    //        case 2:
    //            var resultado2 = _fraudeFinancieroFastTreeRandom.Predecir(convertTrans);

    //            return await Task.FromResult(new TransaccionResponseDTO
    //            {
    //                Score = resultado2.Score,
    //                Prediccion = resultado2.PredictedLabel
    //            });

    //        default:
    //            throw new OperationCanceledException("Algoritmo no asignado correctamenmte.");

    //    }
    //}
}
