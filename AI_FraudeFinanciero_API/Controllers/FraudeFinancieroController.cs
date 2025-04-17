using AI_FraudeFinanciero_API.Application;
using AI_FraudeFinanciero_ML.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_FraudeFinanciero_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FraudeFinancieroController : ControllerBase
{
    private readonly TransaccionUseCase _transaccionUseCase;
    public FraudeFinancieroController(TransaccionUseCase transaccionUseCase)
    {
        _transaccionUseCase = transaccionUseCase;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API de Fraude Financiero en funcionamiento");
    }

    [HttpPost("predecir")]
    public IActionResult PredecirTransaccion([FromBody] TransaccionInput input)
    {
        var resultado = _transaccionUseCase.PredecirTransaccion(input);
        return Ok(resultado);
    }
}
