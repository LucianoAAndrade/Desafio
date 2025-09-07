using Application.Requests.Livros;
using Application.Validation;
using AutoMapper;
using Domain.Interfeces;
using Domain.Models;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Handlers.LivrosHendler
{
    public class PostLivroHandler : IRequestHandler<PostLivroRequest, ResultOperationBase>
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IMapper _mapper;
        public PostLivroHandler(ILivroRepository livroRepository, IMapper mapper)
        {
            _livroRepository = livroRepository;
            _mapper = mapper;
        }
        public Task<ResultOperationBase> Handle(PostLivroRequest request, CancellationToken cancellationToken)
        {
            var retorno = new ResultOperationBase();

            try
            {
                LivroValidator.Validate(request.Titulo, request.Autor);

                var livroModel = new LivroModel( 
                    request.Titulo,
                    request.Autor,
                    request.QuantidadeDisponivel
                );
                _livroRepository.Add(livroModel);
                _livroRepository.SaveChanges();
                retorno.Sucesso = true;
                retorno.MensagemPrincipal = "Livro cadastrado com sucesso";
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
