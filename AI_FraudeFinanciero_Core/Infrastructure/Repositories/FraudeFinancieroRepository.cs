using AI_FraudeFinanciero_Core.Domain.Transacciones;
using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace AI_FraudeFinanciero_Core.Infrastructure.Repositories;

public class FraudeFinancieroRepository : ITransaccion
{
    private readonly AppDBContext _dbContext;
    private readonly DbSet<Transaccion> _dbSetTransaccion; 
    private readonly IUnitOfWork _unitOfWork;

    public FraudeFinancieroRepository(AppDBContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _dbSetTransaccion = _dbContext.Set<Transaccion>(); 
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Transaccion>> GetTransacciones()
    {
        return await Task.FromResult(_dbSetTransaccion.ToList());
    }
}
