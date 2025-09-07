using Application.Responses.Emprestimo;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Requests.Emprestimo
{
    public class PostEmprestimoRequest : IRequest<ResultOperation<PostEmprestimoResponse>>
    {
        public int IdLivro { get; set; }
        public DateTime DataDevolucao { get; set; }
    }
}
