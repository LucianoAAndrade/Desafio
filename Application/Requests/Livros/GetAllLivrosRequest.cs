using Application.Responses.Livros;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Requests.Livros
{
    public class GetAllLivrosRequest : IRequest<ResultOperation<List<GetAllLivrosResponse>>>
    {
    }
}
