using Application.Requests.Emprestimo;
using Application.Responses.Emprestimo;
using Domain.Enums;
using Domain.Interfeces;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Handlers
{
    public class GetAllEmprestimosHandler : IRequestHandler<GetAllEmprestimosRequest, ResultOperation<List<GetAllEmprestimosResponse>>>
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        public GetAllEmprestimosHandler(IEmprestimoRepository emprestimoRepository)
        {
            _emprestimoRepository = emprestimoRepository;
        }
        public Task<ResultOperation<List<GetAllEmprestimosResponse>>> Handle(GetAllEmprestimosRequest request, CancellationToken cancellationToken)
        {
            var retorno = new ResultOperation<List<GetAllEmprestimosResponse>>();
            var listaResponse = new List<GetAllEmprestimosResponse>();

            try
            {
                var emprestimos = _emprestimoRepository.GetAllEmprestimos().ToList();
                foreach (var emprestimo in emprestimos)
                {
                    listaResponse.Add(
                    new GetAllEmprestimosResponse()
                    {
                        IdEmprestimo = emprestimo.Id,
                        TituloLivro = emprestimo.Livro.Titulo,
                        AutorLivro = emprestimo.Livro.Autor,
                        DataEmprestimo = emprestimo.DataEmprestimo,
                        DataDevolucao = emprestimo.DataDevolucao,
                        Status = emprestimo.Status == StatusLivroEnum.Ativo ? "Emprestado" : "Devolvido"
                    });
                }
                if (listaResponse.Count > 0)
                {
                    retorno.Modelo = listaResponse;
                    retorno.Sucesso = true;
                }
                else
                    throw new Exception("Nenhum empréstimo encontrado");
            }
            catch (Exception ex)
            {
                throw;
            }
            return Task.FromResult(retorno);
        }
    }
}
