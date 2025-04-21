using AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;
using AI_FraudeFinanciero_Core.Domain.Transacciones;
using AI_FraudeFinanciero_ML.Models;
using AI_FraudeFinanciero_ML.Services;
using Microsoft.ML;
using Moq;

namespace AI_FraudeFinanciero_Test.AI_FraudeFinanciero_ML_Test;

public class ModeloFFServiceTests
{
    private readonly Mock<ITransaccion> _mockTransaccion;
    private readonly Mock<IModeloEntrenamiento> _mockModeloEntrenamiento;
    private readonly ModeloFFService _service;

    public ModeloFFServiceTests()
    {
        _mockTransaccion = new Mock<ITransaccion>();
        _mockModeloEntrenamiento = new Mock<IModeloEntrenamiento>();
        _service = new ModeloFFService(_mockTransaccion.Object, _mockModeloEntrenamiento.Object);
    }

    //[Fact]
    //public async Task Entrenamiento_ShouldReturn1_WhenTrainingSucceeds()
    //{
    //    // Arrange
    //    var cancellationToken = new CancellationToken();
    //    _mockModeloEntrenamiento.Setup(m => m.Delete(It.IsAny<int>(), cancellationToken)).ReturnsAsync(1);
    //    _mockModeloEntrenamiento.Setup(m => m.Add(It.IsAny<ModeloEntrenamiento>(), cancellationToken)).ReturnsAsync(1);
    //    _mockTransaccion.Setup(t => t.GetTransacciones()).ReturnsAsync(new List<Transaccion>
    //    {
    //        new Transaccion
    //        {
    //            Origen = "A",
    //            Destino = "B",
    //            Canal = "Online",
    //            Tipo = "Transferencia",
    //            Monto = 100,
    //            Frecuencia = 1,
    //            TiempoTransaccion = 1.5f,
    //            Score = 0.8f,
    //            IsSospechosa = true
    //        }
    //    });

    //    // Act
    //    var result = await _service.Entrenamiento(cancellationToken);

    //    // Assert
    //    Assert.Equal(1, result);
    //}

    [Fact]
    public async Task Entrenamiento_ShouldReturn0_WhenExceptionThrown()
    {
        // Arrange
        _mockModeloEntrenamiento.Setup(m => m.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Error"));

        // Act
        var result = await _service.Entrenamiento(CancellationToken.None);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task Liberar_ShouldCallDeleteOnce()
    {
        // Arrange
        _mockModeloEntrenamiento.Setup(m => m.Delete(1, default)).ReturnsAsync(1);

        // Act
        await _service.Liberar(default);

        // Assert
        _mockModeloEntrenamiento.Verify(m => m.Delete(1, default), Times.Once);
    }

    //[Fact]
    //public async Task Predecir_ShouldReturnPrediction_WhenModelExists()
    //{
    //    // Arrange
    //    var model = new ModeloEntrenamiento
    //    {
    //        Modelo_Entrenamiento_Id = 2,
    //        Modelo = CreateTrainedModelByteArray()
    //    };
    //    _mockModeloEntrenamiento.Setup(m => m.GetById(2)).ReturnsAsync(model);

    //    var input = new TransaccionInput
    //    {
    //        Origen = "A",
    //        Destino = "B",
    //        Canal = "Online",
    //        Tipo = "Transferencia",
    //        Monto = 100,
    //        Frecuencia = 1,
    //        TiempoTransaccion = 1.5f
    //    };

    //    // Act
    //    var prediction = await _service.Predecir(input);

    //    // Assert
    //    Assert.NotNull(prediction);
    //}

    private byte[] CreateTrainedModelByteArray()
    {
        var mlContext = new MLContext();
        var samples = new List<TransaccionEntrenamiento>
        {
            new TransaccionEntrenamiento
            {
                Origen = "A",
                Destino = "B",
                Canal = "Online",
                Tipo = "Transferencia",
                Monto = 100,
                Frecuencia = 1,
                TiempoTransaccion = 1.5f,
                IsSospechosa = true
            }
        };

        var data = mlContext.Data.LoadFromEnumerable(samples);
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

        using var memoryStream = new System.IO.MemoryStream();
        mlContext.Model.Save(model, data.Schema, memoryStream);
        return memoryStream.ToArray();
    }
}
