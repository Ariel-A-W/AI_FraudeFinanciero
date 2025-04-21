using AI_FraudeFinanciero_Core.Domain.Transacciones;
using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using AI_FraudeFinanciero_Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AI_FraudeFinanciero_Test.AI_FraudeFinanciero_Core_Test;

public class FraudeFinancieroRepositoryTests
{
    private readonly DbContextOptions<AppDBContext> _dbContextOptions;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public FraudeFinancieroRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task GetTransacciones_ShouldReturnListOfTransacciones()
    {
        using var context = new AppDBContext(_dbContextOptions);
        context.AddRange(new List<Transaccion>
        {
            new Transaccion { Transaccion_Id = 1, Monto = 100 },
            new Transaccion { Transaccion_Id = 2, Monto = 200 }
        });
        await context.SaveChangesAsync();

        var repo = new FraudeFinancieroRepository(context, _unitOfWorkMock.Object);
        var result = await repo.GetTransacciones();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
