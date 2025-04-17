namespace AI_FraudeFinanciero_ML.Models;

public class TransaccionInput
{
    public float Monto { get; set; }
    public float Frecuencia { get; set; }
    public float TiempoTransaccion { get; set; }
    public string? Canal { get; set; }
    public string? Tipo { get; set; }
    public string? Origen { get; set; }
    public string? Destino { get; set; }
}
