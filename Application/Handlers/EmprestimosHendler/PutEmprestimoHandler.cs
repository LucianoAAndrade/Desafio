using Application.Requests.Emprestimo;
using Application.Responses.Emprestimo;
using Domain.Enums;
using Domain.Interfeces;
using Domain.Models;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Handlers
{
    public class PutEmprestimoHandler : IRequestHandler<PutEmprestimoRequest, ResultOperation<PutEmprestimoResponse>>
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        public PutEmprestimoHandler(IEmprestimoRepository emprestimoRepository)
        {
            _emprestimoRepository = emprestimoRepository;
        }
        public Task<ResultOperation<PutEmprestimoResponse>> Handle(PutEmprestimoRequest request, CancellationToken cancellationToken)
        {
            var retorno = new ResultOperation<PutEmprestimoResponse>();

            try
            {
                var emprestimo = _emprestimoRepository.GetByIdEmprestimo(request.IdEmprestimo);

                if (emprestimo == null || emprestimo.Status == StatusLivroEnum.Devolvido)
                    throw new Exception("Empréstimo não encontrado");

                emprestimo.Livro.QuantidadeDisponivel += 1;
                emprestimo.DataDevolucao = request.DataDevolucao;
                emprestimo.Status = StatusLivroEnum.Devolvido;

                _emprestimoRepository.Update(emprestimo);
                _emprestimoRepository.SaveChanges();
                retorno.Sucesso = true;
                retorno.MensagemPrincipal = "Devolução realizada com sucesso";
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.MensagemPrincipal = ex.Message;
            }
            return Task.FromResult(retorno);
        }
    }
}
