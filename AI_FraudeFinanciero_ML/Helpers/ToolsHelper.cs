using AI_FraudeFinanciero_ML.Models;
using Microsoft.ML;
using System.Text;

namespace AI_FraudeFinanciero_ML.Helpers;

public static class ToolsHelper
{
    private static void GetModelPreTrainned(List<TransaccionEntrenamiento> lstTrans)
    {
        var lstFraudes = lstTrans.Where(t => t.IsSospechosa).Take(5).ToList();
        var lstNormales = lstTrans.Where(t => !t.IsSospechosa).Take(5).ToList();
        var muestra = lstFraudes.Concat(lstNormales).ToList();

        var sb = new StringBuilder();

        // Encabezados
        sb.AppendLine("Origen,Destino,Canal,Tipo,Monto,Frecuencia,TiempoTransaccion,Score,IsSospechosa");

        // Datos
        foreach (var t in muestra)
        {
            sb.AppendLine($"{t.Origen},{t.Destino},{t.Canal},{t.Tipo},{t.Monto},{t.Frecuencia},{t.TiempoTransaccion},{t.Score},{t.IsSospechosa}");
        }

        // Resultado como string (para copiar y pegar)
        string csvTexto = sb.ToString();
        System.Diagnostics.Debug.WriteLine(csvTexto);
    }

    public static void GetModelTrainned(MLContext mlContext, IDataView data, IEstimator<ITransformer> pipeline, int maxFilas = 10)
    {
        var transformador = pipeline.Fit(data);
        var transformedData = transformador.Transform(data);

        // Convertimos a objetos legibles
        var preview = transformedData.Preview(maxRows: 10); // Podés ajustar la cantidad

        var sb = new StringBuilder();

        // Encabezados
        sb.AppendLine(string.Join(",", preview.Schema.Select(c => c.Name)));

        // Filas de datos
        foreach (var row in preview.RowView)
        {
            var valores = row.Values.Select(v => v.Value?.ToString()?.Replace(",", ".") ?? ""); // Cuidamos las comas
            sb.AppendLine(string.Join(",", valores));
        }

        string resultadoCSV = sb.ToString();
        System.Diagnostics.Debug.WriteLine(resultadoCSV);
    }
}
