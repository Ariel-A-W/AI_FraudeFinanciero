using AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;
using AI_FraudeFinanciero_Core.Infrastructure.DBContexts;
using AI_FraudeFinanciero_Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AI_FraudeFinanciero_Test.AI_FraudeFinanciero_Core_Test;

public class ModelosEntrenamientosRepositoryTests
{
    private readonly DbContextOptions<AppDBContext> _dbContextOptions;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ModelosEntrenamientosRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Add_ShouldReturn1_WhenSuccess()
    {
        using var context = new AppDBContext(_dbContextOptions);
        var repository = new ModelosEntrenamientosRepository(context, _unitOfWorkMock.Object);

        var modelo = new ModeloEntrenamiento
        {
            Modelo_Entrenamiento_Id = 1,
            Nombre = "Modelo 1",
            Creacion = DateTime.Now,
            Modelo = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }
        };

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await repository.Add(modelo, CancellationToken.None);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectModel()
    {
        using var context = new AppDBContext(_dbContextOptions);
        context.Add(new ModeloEntrenamiento
        {
            Modelo_Entrenamiento_Id = 1,
            Nombre = "TestModel",
            Creacion = DateTime.Now,
            Modelo = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }
        });
        await context.SaveChangesAsync();

        var repo = new ModelosEntrenamientosRepository(context, _unitOfWorkMock.Object);

        var result = await repo.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("TestModel", result.Nombre);
    }

    [Fact]
    public async Task Delete_ShouldReturn1_WhenSuccess()
    {
        using var context = new AppDBContext(_dbContextOptions);
        var repo = new ModelosEntrenamientosRepository(context, _unitOfWorkMock.Object);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await repo.Delete(99, CancellationToken.None); // simulación

        Assert.Equal(1, result);
    }
}
