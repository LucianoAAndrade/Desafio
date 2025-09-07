using Application.Requests.Livros;
using ControleEstoqueAPI.Controllers.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOpea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ApiController
    {
        public LivroController(IMediator mediatR) : base(mediatR) {}

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GetAllLivrosRequest request = new GetAllLivrosRequest();
            return await Execute(() => mediator.Send(request).Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new GetLivroIdRequest() { Id = id };
            return await Execute(() => mediator.Send(request).Result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostLivroRequest request)
        {
            return await Execute(() => mediator.Send(request).Result);
        }
    }
}
