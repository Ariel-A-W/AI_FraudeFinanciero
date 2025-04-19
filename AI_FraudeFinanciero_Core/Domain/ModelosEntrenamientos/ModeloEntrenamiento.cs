namespace AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;

public class ModeloEntrenamiento
{
    public int Modelo_Entrenamiento_Id { get; set; }
    public string? Nombre { get; set; }
    public DateTime Creacion { get; set; } = DateTime.Now;
    public byte[] Modelo { get; set; } = Array.Empty<byte>();
}
