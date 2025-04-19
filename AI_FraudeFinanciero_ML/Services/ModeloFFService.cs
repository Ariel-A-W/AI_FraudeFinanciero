using AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;
using AI_FraudeFinanciero_Core.Domain.Transacciones;
using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_ML.Models;
using Microsoft.ML;

namespace AI_FraudeFinanciero_ML.Services;

public class ModeloFFService : IModeloFFService
{
    private readonly ITransaccion _transaccion;
    private readonly IModeloEntrenamiento _modeloEntrenamiento;

    public ModeloFFService(ITransaccion transaccion, IModeloEntrenamiento modeloEntrenamiento)
    {
        _transaccion = transaccion;
        _modeloEntrenamiento = modeloEntrenamiento;
    }

    public async Task<int> Entrenamiento(CancellationToken cancellationToken)
    {
        try
        {
            await _modeloEntrenamiento.Delete(1, cancellationToken);

            var mlContext = new MLContext();

            var lstTrans = new List<TransaccionEntrenamiento>();

            foreach (var trans in await _transaccion.GetTransacciones())
            {
                lstTrans.Add(new TransaccionEntrenamiento
                {
                    Origen = trans.Origen!,
                    Destino = trans.Destino!,
                    Canal = trans.Canal!,
                    Tipo = trans.Tipo!,
                    Monto = trans.Monto,
                    Frecuencia = trans.Frecuencia,
                    TiempoTransaccion = trans.TiempoTransaccion,
                    Score = trans.Score, // <- usado solo para entrenamiento
                    IsSospechosa = trans.IsSospechosa // <- etiqueta
                });
            }

            var data = mlContext.Data.LoadFromEnumerable(lstTrans);

            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new[]
            {
            new InputOutputColumnPair("OrigenEncoded", "Origen"),
            new InputOutputColumnPair("DestinoEncoded", "Destino"),
            new InputOutputColumnPair("CanalEncoded", "Canal"),
            new InputOutputColumnPair("TipoEncoded", "Tipo")
        })
            .Append(mlContext.Transforms.Concatenate("Features",
                "OrigenEncoded", "DestinoEncoded", "CanalEncoded", "TipoEncoded",
                nameof(TransaccionEntrenamiento.Monto),
                nameof(TransaccionEntrenamiento.Frecuencia),
                nameof(TransaccionEntrenamiento.TiempoTransaccion)))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(TransaccionEntrenamiento.IsSospechosa),
                featureColumnName: "Features"));

            var model = pipeline.Fit(data);

            var memoryStream = new MemoryStream();
            mlContext.Model.Save(model, data.Schema, memoryStream);
            memoryStream.Position = 0;

            await _modeloEntrenamiento.Add(
                new ModeloEntrenamiento()
                {
                    Modelo_Entrenamiento_Id = 1,
                    Nombre = $"Entrenamiento-{DateTime.UtcNow}",
                    Creacion = DateTime.UtcNow,
                    Modelo = memoryStream.ToArray()
                },
                cancellationToken
            );

            return await Task.FromResult(1);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en Entrenamiento: {ex.Message}");
            return await Task.FromResult(0);
        }
    }

    public async Task Liberar(CancellationToken cancellationToken)
    {
        await _modeloEntrenamiento.Delete(1, default);
        await Task.CompletedTask;
    }

    public async Task<TransaccionPrediction> Predecir(TransaccionInput input)
    {
        var modelDB = await _modeloEntrenamiento.GetById(1);

        if (modelDB == null)
            return await Task.FromResult(new TransaccionPrediction());

        using var memoryStream = new MemoryStream(modelDB.Modelo);

        var mlContext = new MLContext();

        var loaadedModel = mlContext.Model.Load(memoryStream, out var _);

        var engine = mlContext.Model.CreatePredictionEngine<
            TransaccionInput, TransaccionPrediction
        >(loaadedModel);

        return await Task.FromResult(engine.Predict(input));
    }
}
