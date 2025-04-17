namespace AI_FraudeFinanciero_Core.Domain;

public class Transaccion
{
    public int Transaccion_Id { get; set; }
    public DateTime Fecha { get; set; }
    public string? Origen { get; set; }
    public string? Destino { get; set; }
    public string? Canal { get; set; }
    public string? Tipo { get; set; }
    public float Monto { get; set; }
    public float Frecuencia { get; set; }
    public float TiempoTransaccion { get; set; }
    public float Score { get; set; }
    public bool IsSospechosa { get; set; }
}
