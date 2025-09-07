using Application.Requests.Livros;
using Application.Responses.Livros;
using Domain.Interfeces;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Handlers.LivrosHendler
{
    public class GetLivroIdHandler : IRequestHandler<GetLivroIdRequest, ResultOperation<GetLivroIdResponse>>
    {
        private readonly ILivroRepository _livroRepository;
        public GetLivroIdHandler(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }
        public Task<ResultOperation<GetLivroIdResponse>> Handle(GetLivroIdRequest request, CancellationToken cancellationToken)
        {
            var retorno = new ResultOperation<GetLivroIdResponse>();

            try
            {
                var livro = _livroRepository.GetById(request.Id);
                if (livro != null)
                {
                    retorno.Modelo = new GetLivroIdResponse()
                    {
                        Titulo = livro.Titulo,
                        Autor = livro.Autor,
                        QuantidadeDisponivel = livro.QuantidadeDisponivel
                    };
                    retorno.Sucesso = true;
                    retorno.MensagemPrincipal = "Livro encontrado com sucesso";
                }
                else
                    throw new Exception("Livro não encontrado");

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
