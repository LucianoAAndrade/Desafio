using Domain.Interfeces;
using Domain.Models;
using Infra.Data.Config;
using Infra.Modelo.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repository
{
    public class EmprestimoRepository : Repository<EmprestimoModel>, IEmprestimoRepository
    {
        public EmprestimoRepository(Context context) : base(context)
        {
        }

        public IQueryable<EmprestimoModel> GetAllEmprestimos()
        {
            return DbSet
                .Include(m => m.Livro);
        }

        public EmprestimoModel GetByIdEmprestimo(int idEmprestimo)
        {
            return DbSet
                .Include(m => m.Livro)
                .FirstOrDefault(m => m.Id == idEmprestimo);
        }

        public EmprestimoModel GetByIdLivro(int idLivro)
        {
            return DbSet
                .Include(m => m.Livro)
                .FirstOrDefault(m => m.LivroId == idLivro);
        }
    }
}
