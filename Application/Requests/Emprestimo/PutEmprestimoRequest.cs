using Application.Responses.Emprestimo;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Requests.Emprestimo
{
    public class PutEmprestimoRequest : IRequest<ResultOperation<PutEmprestimoResponse>>
    {
        public int IdEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
    }
}
