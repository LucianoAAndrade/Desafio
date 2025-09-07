using Application.Responses.Livros;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Requests.Livros
{
    public class GetLivroIdRequest : IRequest<ResultOperation<GetLivroIdResponse>>
    {
        public int Id { get; set; }
    }
}
