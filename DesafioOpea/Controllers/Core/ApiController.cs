using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Utils.OperationResult;

namespace ControleEstoqueAPI.Controllers.Core
{
    [ApiController]
  public abstract class ApiController : ControllerBase
  {
    public readonly IMediator mediator;

    protected ApiController(IMediator _mediator)
    {
      mediator = _mediator;
    }
    protected async Task<IActionResult> Execute(Func<object> funcao)
    {
      try
      {
        var retorno = funcao.Invoke();

        if (retorno is ResultOperationBase)
        {
          return await CriarActionResult(retorno as ResultOperationBase);
        }

        return Ok(retorno);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    protected async Task<IActionResult> CriarActionResult(ResultOperationBase resultado) => StatusCode(resultado.StatusCode, resultado);
  }
}
