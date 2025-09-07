using Domain.Models;

namespace Domain.Interfeces
{
    public interface IEmprestimoRepository : IRepository<EmprestimoModel>
    {
        IQueryable<EmprestimoModel> GetAllEmprestimos();
        EmprestimoModel GetByIdLivro(int idLivro);
        EmprestimoModel GetByIdEmprestimo(int idEmprestimo);
    }
}
