using AI_FraudeFinanciero_Core.Domain.Transacciones;
using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_ML.Models;
using Microsoft.ML;

namespace AI_FraudeFinanciero_ML.Services;

public class ModeloFraudeFinancieroFastTreeRandomService : IModeloFraudeFinancieroFastTreeRandomService
{
    private readonly ITransaccion _transaccion;
    private readonly MLContext _mlContext;
    private readonly ITransformer? _model; // Marked as nullable

    public ModeloFraudeFinancieroFastTreeRandomService(ITransaccion transaccion, Stream? memoryStream = null) // Marked Stream as nullable
    {
        _transaccion = transaccion;
        _mlContext = new MLContext();

        memoryStream = Entrenamiento().GetAwaiter().GetResult();

        if (memoryStream != null)
        {
            _model = _mlContext.Model.Load(memoryStream, out var _);
        }
    }

    public async Task<MemoryStream> Entrenamiento()
    {
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

        var data = _mlContext.Data.LoadFromEnumerable(lstTrans);

        // Construcción del pipeline de transformación de datos
        var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding(new[]
        {
            new InputOutputColumnPair("OrigenEncoded", "Origen"),
            new InputOutputColumnPair("DestinoEncoded", "Destino"),
            new InputOutputColumnPair("CanalEncoded", "Canal"),
            new InputOutputColumnPair("TipoEncoded", "Tipo")
        })
        .Append(_mlContext.Transforms.Concatenate("Features",
            "OrigenEncoded", "DestinoEncoded", "CanalEncoded", "TipoEncoded",
            nameof(TransaccionEntrenamiento.Monto),
            nameof(TransaccionEntrenamiento.Frecuencia),
            nameof(TransaccionEntrenamiento.TiempoTransaccion)));

        // Modelos de clasificación: SDCA, FastTree, RandomForest
        var modelType = "FastTree"; // Puedes cambiar esto a "RandomForest" para usar RandomForest
        IEstimator<ITransformer> trainer;

        switch (modelType)
        {
            case "FastTree":
                trainer = _mlContext.BinaryClassification.Trainers.FastTree(
                    labelColumnName: nameof(TransaccionEntrenamiento.IsSospechosa),
                    featureColumnName: "Features");
                break;

            case "RandomForest":
                trainer = _mlContext.BinaryClassification.Trainers.FastForest(
                    labelColumnName: nameof(TransaccionEntrenamiento.IsSospechosa),
                    featureColumnName: "Features");
                break;

            default:
                // Default to SDCA if no valid model type is selected
                trainer = _mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: nameof(TransaccionEntrenamiento.IsSospechosa),
                    featureColumnName: "Features");
                break;
        }

        // Encadena el pipeline con el modelo
        var model = pipeline.Append(trainer).Fit(data);

        var memoryStream = new MemoryStream();
        _mlContext.Model.Save(model, data.Schema, memoryStream);
        memoryStream.Position = 0;

        return await Task.FromResult(memoryStream);
    }

    public TransaccionPrediction Predecir(TransaccionInput input)
    {
        if (_model == null)
            throw new InvalidOperationException(
                "El modelo no ha sido cargado o entrenado correctamente."
            );

        var engine = _mlContext.Model.CreatePredictionEngine<TransaccionInput, TransaccionPrediction>(_model);
        return engine.Predict(input);
    }
}

