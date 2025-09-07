using Application.Responses.Emprestimo;
using Domain.Utils.OperationResult;
using MediatR;

namespace Application.Requests.Emprestimo
{
    public class GetAllEmprestimosRequest : IRequest<ResultOperation<List<GetAllEmprestimosResponse>>>
    {
    }
}
