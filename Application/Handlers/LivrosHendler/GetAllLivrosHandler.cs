using Application.Requests.Livros;
using Application.Responses.Livros;
using Domain.Interfeces;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Handlers.LivrosHendler
{
    public class GetAllLivrosHandler : IRequestHandler<GetAllLivrosRequest, ResultOperation<List<GetAllLivrosResponse>>>
    {
        private readonly ILivroRepository _livroRepository;
        public GetAllLivrosHandler(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }
        public Task<ResultOperation<List<GetAllLivrosResponse>>> Handle(GetAllLivrosRequest request, CancellationToken cancellationToken)
        {
            var retorno = new ResultOperation<List<GetAllLivrosResponse>>();
            var listaResponse = new List<GetAllLivrosResponse>();

            try
            {
                var livros = _livroRepository.GetAll().ToList();

                foreach (var livro in livros)
                {
                    listaResponse.Add(
                    new GetAllLivrosResponse()
                    {
                        Id = livro.Id,
                        Titulo = livro.Titulo,
                        Autor = livro.Autor,
                        QuantidadeDisponivel = livro.QuantidadeDisponivel
                    });
                }

                if (listaResponse.Count > 0)
                {
                    retorno.Modelo = listaResponse;
                    retorno.Sucesso = true;
                }
                else
                    throw new Exception("Nenhum livro encontrado");

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
