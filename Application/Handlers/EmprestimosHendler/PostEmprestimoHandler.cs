using Application.Requests.Emprestimo;
using Application.Responses.Emprestimo;
using Domain.Enums;
using Domain.Interfeces;
using Domain.Models;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Handlers
{
    public class PostEmprestimoHandler : IRequestHandler<PostEmprestimoRequest, ResultOperation<PostEmprestimoResponse>>
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly ILivroRepository _livroRepository;
        public PostEmprestimoHandler(IEmprestimoRepository emprestimoRepository,
            ILivroRepository livroRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _livroRepository = livroRepository;
        }
        public Task<ResultOperation<PostEmprestimoResponse>> Handle(PostEmprestimoRequest request, CancellationToken cancellationToken)
        {
            var retorno = new ResultOperation<PostEmprestimoResponse>();

            try
            {
                var livro = _livroRepository.GetById(request.IdLivro);

                var emprestimo = _emprestimoRepository.GetByIdLivro(request.IdLivro);

                if (emprestimo != null 
                    && emprestimo?.Livro.QuantidadeDisponivel < 1 || livro == null || livro.QuantidadeDisponivel < 1)
                    throw new Exception("Empréstimo não pode ser realizado");

                livro.QuantidadeDisponivel -= 1;

                var emprestimoModel = new EmprestimoModel(
                    request.IdLivro,
                    DateTime.Now,
                    request.DataDevolucao,
                    StatusLivroEnum.Ativo,
                    livro
                );
                _emprestimoRepository.Add(emprestimoModel);
                _emprestimoRepository.SaveChanges();
                retorno.Sucesso = true;
                retorno.MensagemPrincipal = "Empréstimo realizado com sucesso";
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
