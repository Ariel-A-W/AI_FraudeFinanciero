using AI_FraudeFinanciero_ML.Models;
using Microsoft.ML;
using AI_FraudeFinanciero_ML.IServices;
using AI_FraudeFinanciero_Core.Domain;

namespace AI_FraudeFinanciero_ML.Services;

public class ModeloFraudeFinancieroService : IModeloFraudeFinancieroService
{
    private readonly ITransaccion _transaccion;
    private readonly MLContext _mlContext;
    private readonly ITransformer? _model; // Marked as nullable

    public ModeloFraudeFinancieroService(ITransaccion transaccion, Stream? memoryStream = null) // Marked Stream as nullable
    {
        _transaccion = transaccion;

        _mlContext = new MLContext();

        memoryStream = Entrenamiento();

        if (memoryStream != null)
        {            
            _model = _mlContext.Model.Load(memoryStream, out var _);
        }
    }

    public MemoryStream Entrenamiento()
    {
        var lstTrans = new List<TransaccionEntrenamiento>();

        foreach (var trans in _transaccion.GetTransacciones())
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
            nameof(TransaccionEntrenamiento.TiempoTransaccion)))
        .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
            labelColumnName: nameof(TransaccionEntrenamiento.IsSospechosa),
            featureColumnName: "Features"));

        var model = pipeline.Fit(data);

        var memoryStream = new MemoryStream();
        _mlContext.Model.Save(model, data.Schema, memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
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
