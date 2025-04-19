namespace AI_FraudeFinanciero_Core.Domain.Transacciones;

public interface ITransaccion
{
    Task<List<Transaccion>> GetTransacciones();
}
