using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Requests.Livros
{
    public class PostLivroRequest : IRequest<ResultOperationBase>
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public short QuantidadeDisponivel { get; set; }
    }
}
