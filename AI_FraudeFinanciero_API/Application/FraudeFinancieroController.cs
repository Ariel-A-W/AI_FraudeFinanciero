using AI_FraudeFinanciero_API.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AI_FraudeFinanciero_API.Application;

[ApiController]
[Route("api/[controller]")]
public class FraudeFinancieroController : ControllerBase
{
    private readonly TransaccionUseCase _transaccionUseCase;
    public FraudeFinancieroController(TransaccionUseCase transaccionUseCase)
    {
        _transaccionUseCase = transaccionUseCase;
    }

    [HttpPost("entrenar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetEntrnenar(
        CancellationToken cancellationToken)
    {
        var trainned = await _transaccionUseCase.EntrenarTransacciones(cancellationToken);

        if (trainned == 0)
            return await Task.FromResult(NotFound("No se puedo entrenar el modelo."));

        return await Task.FromResult(Ok("Entrenamiento satisfactorio."));
    }

    [HttpPost("predecir")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransaccionResponseDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TransaccionResponseDTO>> GetPredecir(
        [FromBody] TransaccionRequestDTO transaccion,
        CancellationToken cancellationToken)
    {
        var resultado = _transaccionUseCase.PredecirTransaccion(
            transaccion, cancellationToken
        );
        return await Task.FromResult(Ok(resultado.Result));
    }
}
