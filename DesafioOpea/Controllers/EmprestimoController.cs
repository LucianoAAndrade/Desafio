using Application.Requests.Emprestimo;
using ControleEstoqueAPI.Controllers.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOpea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ApiController
    {
        public EmprestimoController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new GetAllEmprestimosRequest();
            return await Execute(() => mediator.Send(request).Result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostEmprestimoRequest request)
        {
            return await Execute(() => mediator.Send(request).Result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]PutEmprestimoRequest request)
        {
            return await Execute(() => mediator.Send(request).Result);
        }
    }
}
